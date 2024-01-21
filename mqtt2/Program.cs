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
using mqtt2;

namespace mqtt
{
    /// <summary>
    /// Main program class for handling MQTT communication and database operations.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main entry point of the program.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        static async Task Main(string[] args)
        {
            var sharedDevicesFactory = new MqttFactory();
            var sharedDevicesClient = sharedDevicesFactory.CreateMqttClient();

            var personalDeviceFactory = new MqttFactory();
            var personalDeviceClient = personalDeviceFactory.CreateMqttClient();

            // Use TCP connection.
            var sharedDevicesOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("eu1.cloud.thethings.network", 8883) // Port can be 1883 or 8883
                .WithCredentials("project-software-engineering@ttn", "NNSXS.DTT4HTNBXEQDZ4QYU6SG73Q2OXCERCZ6574RVXI.CQE6IG6FYNJOO2MOFMXZVWZE4GXTCC2YXNQNFDLQL4APZMWU6ZGA")
                .WithTls() // For TLS encryption
                .Build();

            var personalDeviceOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("eu1.cloud.thethings.network", 1883) // Port can be 1883 or 8883
                .WithCredentials("lorasensor-saxion-group5@ttn", "NNSXS.P64MFYJMU272L7QRWCJYOUVDJVVYPSNWGFSNTMY.YCCCN63LSNSF74ORG5FVUQPZRUH3MHMITLOQLAVX3HKNELT5NPKA")
                //.WithTls() // For TLS encryption
                .Build();

            personalDeviceClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH PERSONAL DEVICE SERVER ###");

                // Subscribe to a topic
                await personalDeviceClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("#").Build());

                Console.WriteLine("### SUBSCRIBED TO PERSONAL DEVICE ###");
            });

            personalDeviceClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from personal device");
            });

            personalDeviceClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE FROM PERSONAL DEVICE ###");
                Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                // TODO: Deserialize JSON payload
                var payloadJson = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                var parsedPayload = JsonConvert.DeserializeObject<MKRData>(payloadJson);
                UploadToDatabase(parsedPayload);
            });

            sharedDevicesClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("### CONNECTED WITH SHARED DEVICES SERVER ###");

                // Subscribe to a topic
                await sharedDevicesClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("v3/project-software-engineering@ttn/devices/#").Build());

                Console.WriteLine("### SUBSCRIBED TO SHARED DEVICES ###");
            });

            sharedDevicesClient.UseDisconnectedHandler(e =>
            {
                Console.WriteLine("Disconnected from shared devices");
            });

            sharedDevicesClient.UseApplicationMessageReceivedHandler(e =>
            {
                Console.WriteLine("### RECEIVED APPLICATION MESSAGE FROM SHARED DEVICES ###");
                string topic = e.ApplicationMessage.Topic;
                Console.WriteLine($"+ Topic = {topic}");
                int a = topic.IndexOf("devices/") + "devices/".Length;
                int b = topic.IndexOf("/up", a);
                string device = topic.Substring(a, b - a);
                Console.WriteLine("Device ID: {0}", device);
                Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
                Console.WriteLine();

                // Deserialize JSON payload
                var payloadJson = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                if (device.StartsWith("lht"))
                {
                    var parsedLHTPayload = JsonConvert.DeserializeObject<LHTData>(payloadJson);
                    UploadToDatabase(parsedLHTPayload);
                }
                else if (device.StartsWith("mkr"))
                {
                    var parsedMKRPayload = JsonConvert.DeserializeObject<MKRData>(payloadJson);
                    UploadToDatabase(parsedMKRPayload);
                }

                //UploadToDatabase(parsedPayload);
            });


            await personalDeviceClient.ConnectAsync(personalDeviceOptions);
            await sharedDevicesClient.ConnectAsync(sharedDevicesOptions);

            Console.ReadLine();

            await personalDeviceClient.DisconnectAsync();
            await sharedDevicesClient.DisconnectAsync();
        }

        /// <summary>
        /// Uploads LHT sensor data to the database.
        /// </summary>
        /// <param name="parsedPayload">Parsed LHT sensor data.</param>
        static void UploadToDatabase(LHTData parsedPayload)
        {
            try
            {
                string connectionString = "Server=projectgroup5saxion.database.windows.net;Database=project_software_engineering;User ID=project_group5_saxion;Password=weatherstation5!;";

                string gatewayInsertQuery = "IF NOT EXISTS(SELECT 1 FROM gateway WHERE gateway_id = @gateway_id) " +
                                            "BEGIN " +
                                            "INSERT INTO gateway (gateway_id, latitude, longitude, altitude) " +
                                            "VALUES (@gateway_id, @latitude, @longitude, @altitude); " +
                                            "END";

                string deviceInsertQuery = "IF EXISTS(SELECT 1 FROM device WHERE device_id = @device_id) " +
                                            "BEGIN " +
                                            "UPDATE device SET gateway_id = @gateway_id, battery_status = @battery_status, BatV = @BatV WHERE device_id = @device_id; " +
                                            "END " +
                                            "ELSE " +
                                            "BEGIN " +
                                            "INSERT INTO device (device_id, gateway_id, battery_status, BatV) " +
                                            "VALUES (@device_id, @gateway_id, @battery_status, @BatV); " +
                                            "END";

                string sensorDataInsertQuery = "INSERT INTO sensor_data (device_id, temperature_in, temperature_out, humidity, ambient_light, barometric_pressure, date_time)" +
                                               "VALUES (@device_id, @temperature_in, @temperature_out, @humidity, @ambient_light, @barometric_pressure, @date_time)";

                string gatewayMetadataInsertQuery = "INSERT INTO gateway_metadata (rssi, snr, airtime, gateway_id)" +
                                                    "VALUES (@rssi, @snr, @airtime, @gateway_id)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand gatewayInsertCommand = new SqlCommand(gatewayInsertQuery, connection))
                    {
                        string gatewayID = parsedPayload.uplink_message.rx_metadata[0].gateway_ids.gateway_id;
                        Console.WriteLine(gatewayID);
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
                        int batteryStatus = parsedPayload.uplink_message.decoded_payload.Bat_status;
                        double batteryVoltage = parsedPayload.uplink_message.decoded_payload.BatV;

                        deviceInsertCommand.Parameters.AddWithValue("@battery_status", batteryStatus);
                        deviceInsertCommand.Parameters.AddWithValue("@BatV", batteryVoltage);
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
                    using (SqlCommand gatewayMetadataInsertCommand = new SqlCommand(gatewayMetadataInsertQuery, connection))
                    {
                        double airtime = double.Parse(parsedPayload.uplink_message.consumed_airtime.Substring(0, parsedPayload.uplink_message.consumed_airtime.Length - 2));
                        double rssi = parsedPayload.uplink_message.rx_metadata[0].rssi;
                        double snr = parsedPayload.uplink_message.rx_metadata[0].snr;
                        string gatewayID = parsedPayload.uplink_message.rx_metadata[0].gateway_ids.gateway_id;

                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@gateway_id", gatewayID);
                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@rssi", rssi);
                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@snr", snr);
                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@airtime", airtime);
                        connection.Open();
                        int rowsAffected = gatewayMetadataInsertCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("gateway_metadata: Data insertion successful.\n");
                        }
                        else
                        {
                            Console.WriteLine("gateway_metadata: Data insertion failed.\n");
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

        /// <summary>
        /// Uploads MKR sensor data to the database.
        /// </summary>
        /// <param name="parsedPayload">Parsed MKR sensor data.</param>
        static void UploadToDatabase(MKRData parsedPayload)
        {
            try
            {
                string connectionString = "Server=projectgroup5saxion.database.windows.net;Database=project_software_engineering;User ID=project_group5_saxion;Password=weatherstation5!;";

                string gatewayInsertQuery = "IF NOT EXISTS(SELECT 1 FROM gateway WHERE gateway_id = @gateway_id) " +
                                            "BEGIN " +
                                            "INSERT INTO gateway (gateway_id, latitude, longitude, altitude) " +
                                            "VALUES (@gateway_id, @latitude, @longitude, @altitude); " +
                                            "END";

                string deviceInsertQuery = "IF EXISTS(SELECT 1 FROM device WHERE device_id = @device_id) " +
                                            "BEGIN " +
                                            "UPDATE device SET gateway_id = @gateway_id, battery_status = @battery_status, BatV = @BatV WHERE device_id = @device_id; " +
                                            "END " +
                                            "ELSE " +
                                            "BEGIN " +
                                            "INSERT INTO device (device_id, gateway_id, battery_status, BatV) " +
                                            "VALUES (@device_id, @gateway_id, @battery_status, @BatV); " +
                                            "END";

                string sensorDataInsertQuery = "INSERT INTO sensor_data (device_id, temperature_in, temperature_out, humidity, ambient_light, barometric_pressure, date_time)" +
                                               "VALUES (@device_id, @temperature_in, @temperature_out, @humidity, @ambient_light, @barometric_pressure, @date_time)";

                string gatewayMetadataInsertQuery = "INSERT INTO gateway_metadata (rssi, snr, airtime, gateway_id)" +
                                                    "VALUES (@rssi, @snr, @airtime, @gateway_id)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand gatewayInsertCommand = new SqlCommand(gatewayInsertQuery, connection))
                    {
                        string gatewayID = parsedPayload.uplink_message.rx_metadata[0].gateway_ids.gateway_id;
                        Console.WriteLine(gatewayID);
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

                        deviceInsertCommand.Parameters.AddWithValue("@battery_status", DBNull.Value);
                        deviceInsertCommand.Parameters.AddWithValue("@BatV", DBNull.Value);
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
                    using (SqlCommand sensorDataInsertCommand = new SqlCommand(sensorDataInsertQuery, connection))
                    {
                        string deviceID = parsedPayload.end_device_ids.device_id;
                        double temperature = parsedPayload.uplink_message.decoded_payload.temperature;
                        double humidity = parsedPayload.uplink_message.decoded_payload.humidity;
                        int ambientLight = parsedPayload.uplink_message.decoded_payload.light;
                        int pressure = parsedPayload.uplink_message.decoded_payload.pressure;
                        DateTime sentAt = parsedPayload.uplink_message.rx_metadata[0].received_at;

                        sensorDataInsertCommand.Parameters.AddWithValue("@device_id", deviceID);
                        sensorDataInsertCommand.Parameters.AddWithValue("@temperature_in", temperature);
                        sensorDataInsertCommand.Parameters.AddWithValue("@temperature_out", DBNull.Value);
                        sensorDataInsertCommand.Parameters.AddWithValue("@humidity", humidity);
                        sensorDataInsertCommand.Parameters.AddWithValue("@ambient_light", ambientLight);
                        sensorDataInsertCommand.Parameters.AddWithValue("@barometric_pressure", pressure);
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
                    using (SqlCommand gatewayMetadataInsertCommand = new SqlCommand(gatewayMetadataInsertQuery, connection))
                    {
                        double airtime = double.Parse(parsedPayload.uplink_message.consumed_airtime.Substring(0, parsedPayload.uplink_message.consumed_airtime.Length - 2));
                        double rssi = parsedPayload.uplink_message.rx_metadata[0].rssi;
                        double snr = parsedPayload.uplink_message.rx_metadata[0].snr;
                        string gatewayID = parsedPayload.uplink_message.rx_metadata[0].gateway_ids.gateway_id;

                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@gateway_id", gatewayID);
                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@rssi", rssi);
                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@snr", snr);
                        gatewayMetadataInsertCommand.Parameters.AddWithValue("@airtime", airtime);
                        connection.Open();
                        int rowsAffected = gatewayMetadataInsertCommand.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("gateway_metadata: Data insertion successful.\n");
                        }
                        else
                        {
                            Console.WriteLine("gateway_metadata: Data insertion failed.\n");
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
}
