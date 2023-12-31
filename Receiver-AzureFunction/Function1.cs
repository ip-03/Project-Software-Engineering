using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Receiver_AzureFunction
{
    public class receive_upload
    {
        [FunctionName("receive_upload")]
        public async Task RunAsync([TimerTrigger("0 3,7,19,23,27,39,43,47,59 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Run MQTT client
            var semaphore = new SemaphoreSlim(0);

            var factory = new MqttFactory();
            var client = factory.CreateMqttClient();

            // Use TCP connection.
            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("eu1.cloud.thethings.network", 8883) // Port can be 1883 or 8883
                .WithCredentials("project-software-engineering@ttn", "NNSXS.DTT4HTNBXEQDZ4QYU6SG73Q2OXCERCZ6574RVXI.CQE6IG6FYNJOO2MOFMXZVWZE4GXTCC2YXNQNFDLQL4APZMWU6ZGA")
                .WithTls() // For TLS encryption
                .Build();

            client.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("v3/project-software-engineering@ttn/devices/#").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            });

            client.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected");
            });

            client.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                // Deserialize JSON payload
                var payloadJson = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var parsedPayload = JsonConvert.DeserializeObject<DeviceData>(payloadJson);

                UploadToDatabase(parsedPayload);
                semaphore.Release();
            });

            await client.ConnectAsync(options);

            await semaphore.WaitAsync();

            await client.DisconnectAsync();
        }
        static void UploadToDatabase(DeviceData parsedPayload)
        {
            try
            {
                string connectionString = "Server=projectgroup5saxion.database.windows.net;Database=project_software_engineering;User ID=project_group5_saxion;Password=weatherstation5!;";

                string gatewayInsertQuery = "IF EXISTS(SELECT 1 FROM gateway WHERE gateway_id = @gateway_id) " +
                                            "BEGIN " +
                                            "UPDATE gateway SET rssi = @rssi, snr = @snr, avg_airtime = @avg_airtime WHERE gateway_id = @gateway_id; " +
                                            "END " +
                                            "ELSE " +
                                            "BEGIN " +
                                            "INSERT INTO gateway (gateway_id, latitude, longitude, altitude, rssi, snr, avg_airtime) " +
                                            "VALUES (@gateway_id, @latitude, @longitude, @altitude, @rssi, @snr, @avg_airtime); " +
                                            "END";

                string deviceInsertQuery = "IF EXISTS(SELECT 1 FROM device WHERE device_id = @device_id) " +
                                            "BEGIN " +
                                            "UPDATE device SET gateway_id = @gateway_id WHERE device_id = @device_id; " +
                                            "END " +
                                            "ELSE " +
                                            "BEGIN " +
                                            "INSERT INTO device (device_id, gateway_id) " +
                                            "VALUES (@device_id, @gateway_id); " +
                                            "END";

                string batteryStatusInsertQuery = "IF EXISTS(SELECT 1 FROM battery_status WHERE device_id = @device_id) " +
                                                  "BEGIN " +
                                                  "UPDATE battery_status SET battery_status = @battery_status, BatV = @BatV WHERE device_id = @device_id; " +
                                                  "END " +
                                                  "ELSE " +
                                                  "BEGIN " +
                                                  "INSERT INTO battery_status (device_id, battery_status, BatV) " +
                                                  "VALUES (@device_id, @battery_status, @BatV); " +
                                                  "END";

                string sensorDataInsertQuery = "INSERT INTO sensor_data (device_id, temperature_in, temperature_out, humidity, ambient_light, barometric_pressure, date_time)" +
                                               "VALUES (@device_id, @temperature_in, @temperature_out, @humidity, @ambient_light, @barometric_pressure, @date_time)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand gatewayInsertCommand = new SqlCommand(gatewayInsertQuery, connection))
                    {
                        string gatewayID = parsedPayload.uplink_message.rx_metadata[0].gateway_ids.gateway_id;
                        double airtime = double.Parse(parsedPayload.uplink_message.consumed_airtime.Substring(0, parsedPayload.uplink_message.consumed_airtime.Length - 2));
                        double rssi = parsedPayload.uplink_message.rx_metadata[0].rssi;
                        double snr = parsedPayload.uplink_message.rx_metadata[0].snr;
                        double latitude = parsedPayload.uplink_message.rx_metadata[0].location.latitude;
                        double longitude = parsedPayload.uplink_message.rx_metadata[0].location.longitude;
                        int? altitude = parsedPayload.uplink_message.rx_metadata[0].location.altitude;

                        gatewayInsertCommand.Parameters.AddWithValue("@gateway_id", gatewayID);
                        gatewayInsertCommand.Parameters.AddWithValue("@latitude", latitude);
                        gatewayInsertCommand.Parameters.AddWithValue("@longitude", longitude);
                        if (altitude.HasValue)
                        {
                            gatewayInsertCommand.Parameters.AddWithValue("@altitude", altitude.Value);
                        }
                        else
                        {
                            gatewayInsertCommand.Parameters.AddWithValue("@altitude", DBNull.Value);
                        }
                        gatewayInsertCommand.Parameters.AddWithValue("@rssi", rssi);
                        gatewayInsertCommand.Parameters.AddWithValue("@snr", snr);
                        gatewayInsertCommand.Parameters.AddWithValue("@avg_airtime", airtime);

                        connection.Open();
                        int rowsAffected = gatewayInsertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("gateway: Data insertion successful.");
                        }
                        else
                        {
                            Console.WriteLine("gateway: Data insertion failed.");
                        }
                        connection.Close();
                    }
                    using (SqlCommand deviceInsertCommand = new SqlCommand(deviceInsertQuery, connection))
                    {
                        string deviceID = parsedPayload.end_device_ids.device_id;
                        string gatewayID = parsedPayload.uplink_message.rx_metadata[0].gateway_ids.gateway_id;

                        deviceInsertCommand.Parameters.AddWithValue("@device_id", deviceID);
                        deviceInsertCommand.Parameters.AddWithValue("@gateway_id", gatewayID);

                        connection.Open();
                        int rowsAffected = deviceInsertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("device: Data insertion successful.");
                        }
                        else
                        {
                            Console.WriteLine("device: Data insertion failed.");
                        }
                        connection.Close();
                    }
                    using (SqlCommand batteryStatusInsertCommand = new SqlCommand(batteryStatusInsertQuery, connection))
                    {
                        string deviceID = parsedPayload.end_device_ids.device_id;
                        int batteryStatus = parsedPayload.uplink_message.decoded_payload.Bat_status;
                        double batteryVoltage = parsedPayload.uplink_message.decoded_payload.BatV;

                        batteryStatusInsertCommand.Parameters.AddWithValue("@device_id", deviceID);
                        batteryStatusInsertCommand.Parameters.AddWithValue("@battery_status", batteryStatus);
                        batteryStatusInsertCommand.Parameters.AddWithValue("@BatV", batteryVoltage);

                        connection.Open();
                        int rowsAffected = batteryStatusInsertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("battery_status: Data insertion successful.");
                        }
                        else
                        {
                            Console.WriteLine("battery_status: Data insertion failed.");
                        }
                        connection.Close();
                    }
                    using (SqlCommand sensorDataInsertCommand = new SqlCommand(sensorDataInsertQuery, connection))
                    {
                        string deviceID = parsedPayload.end_device_ids.device_id;
                        double temperatureSHT = parsedPayload.uplink_message.decoded_payload.TempC_SHT;
                        double? temperatureDS = parsedPayload.uplink_message.decoded_payload.TempC_DS;
                        double humidity = parsedPayload.uplink_message.decoded_payload.Hum_SHT;
                        int ambientLight = parsedPayload.uplink_message.decoded_payload.ILL_lx;
                        DateTime sentAt = parsedPayload.uplink_message.rx_metadata[0].received_at;

                        sensorDataInsertCommand.Parameters.AddWithValue("@device_id", deviceID);
                        if (temperatureDS.HasValue)
                        {
                            sensorDataInsertCommand.Parameters.AddWithValue("@temperature_out", temperatureDS.Value);
                            sensorDataInsertCommand.Parameters.AddWithValue("@temperature_in", temperatureSHT);
                        }
                        else
                        {
                            sensorDataInsertCommand.Parameters.AddWithValue("@temperature_in", DBNull.Value);
                            sensorDataInsertCommand.Parameters.AddWithValue("@temperature_out", temperatureSHT);
                        }
                        sensorDataInsertCommand.Parameters.AddWithValue("@humidity", humidity);
                        sensorDataInsertCommand.Parameters.AddWithValue("@ambient_light", ambientLight);
                        sensorDataInsertCommand.Parameters.AddWithValue("@barometric_pressure", DBNull.Value);
                        sensorDataInsertCommand.Parameters.AddWithValue("@date_time", sentAt);

                        connection.Open();
                        int rowsAffected = sensorDataInsertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("sensor_data: Data insertion successful.\n");
                        }
                        else
                        {
                            Console.WriteLine("sensor_data: Data insertion failed.\n");
                        }
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    // Define the structure of the JSON payload using C# classes
    public class DeviceData
    {
        public EndDeviceIds end_device_ids { get; set; }
        public DateTime received_at { get; set; }
        public UplinkMessage uplink_message { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int? altitude { get; set; }
    }

    public class GatewayIds
    {
        public string gateway_id { get; set; }
    }

    public class RxMetadata
    {
        public GatewayIds gateway_ids { get; set; }
        public int rssi { get; set; }
        public double snr { get; set; }
        public Location location { get; set; }
        public DateTime received_at { get; set; }
    }
    public class EndDeviceIds
    {
        public string device_id { get; set; }
    }

    public class UplinkMessage
    {
        public DecodedPayload decoded_payload { get; set; }
        public List<RxMetadata> rx_metadata { get; set; }
        public string consumed_airtime { get; set; }
    }

    public class DecodedPayload
    {
        public double BatV { get; set; }
        public int Bat_status { get; set; }
        public double TempC_SHT { get; set; }
        public double? TempC_DS { get; set; }
        public double Hum_SHT { get; set; }
        public int ILL_lx { get; set; }
    }
}