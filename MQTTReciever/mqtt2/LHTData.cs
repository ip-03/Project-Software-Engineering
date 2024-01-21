using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt2
{
    /// <summary>
    /// Represents the data received from an LHT device.
    /// </summary>
    internal class LHTData
    {
        /// <summary>
        /// Gets or sets the end device IDs.
        /// </summary>
        public EndDeviceIds end_device_ids { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the data was received.
        /// </summary>
        public DateTime received_at { get; set; }

        /// <summary>
        /// Gets or sets the uplink message from the LHT device.
        /// </summary>
        public LHTUplinkMessage uplink_message { get; set; }
    }

    /// <summary>
    /// Represents location information (latitude, longitude, altitude).
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double longitude { get; set; }

        /// <summary>
        /// Gets or sets the altitude (nullable).
        /// </summary>
        public int? altitude { get; set; }
    }

    /// <summary>
    /// Represents gateway IDs.
    /// </summary>
    public class GatewayIds
    {
        /// <summary>
        /// Gets or sets the gateway ID.
        /// </summary>
        public string gateway_id { get; set; }
    }

    /// <summary>
    /// Represents receive (RX) metadata.
    /// </summary>
    public class RxMetadata
    {
        /// <summary>
        /// Gets or sets the gateway IDs.
        /// </summary>
        public GatewayIds gateway_ids { get; set; }

        /// <summary>
        /// Gets or sets the Received Signal Strength Indicator (RSSI).
        /// </summary>
        public int rssi { get; set; }

        /// <summary>
        /// Gets or sets the Signal-to-Noise Ratio (SNR).
        /// </summary>
        public double snr { get; set; }

        /// <summary>
        /// Gets or sets the location information.
        /// </summary>
        public Location location { get; set; }

        /// <summary>
        /// Gets or sets the timestamp when the data was received.
        /// </summary>
        public DateTime received_at { get; set; }
    }

    /// <summary>
    /// Represents end device IDs.
    /// </summary>
    public class EndDeviceIds
    {
        /// <summary>
        /// Gets or sets the device ID.
        /// </summary>
        public string device_id { get; set; }
    }

    /// <summary>
    /// Represents an uplink message from an LHT device.
    /// </summary>
    public class LHTUplinkMessage
    {
        /// <summary>
        /// Gets or sets the decoded payload of the LHT uplink message.
        /// </summary>
        public LHTDecodedPayload decoded_payload { get; set; }

        /// <summary>
        /// Gets or sets the list of receive (RX) metadata for the uplink message.
        /// </summary>
        public List<RxMetadata> rx_metadata { get; set; }

        /// <summary>
        /// Gets or sets the airtime consumed by the uplink message.
        /// </summary>
        public string consumed_airtime { get; set; }
    }

    /// <summary>
    /// Represents the decoded payload of an LHT uplink message.
    /// </summary>
    public class LHTDecodedPayload
    {
        /// <summary>
        /// Gets or sets the battery voltage.
        /// </summary>
        public double BatV { get; set; }

        /// <summary>
        /// Gets or sets the battery status.
        /// </summary>
        public int Bat_status { get; set; }

        /// <summary>
        /// Gets or sets the temperature from the SHT sensor.
        /// </summary>
        public double TempC_SHT { get; set; }

        /// <summary>
        /// Gets or sets the temperature from the DS sensor (nullable).
        /// </summary>
        public double? TempC_DS { get; set; }

        /// <summary>
        /// Gets or sets the humidity from the SHT sensor.
        /// </summary>
        public double Hum_SHT { get; set; }

        /// <summary>
        /// Gets or sets the ambient light intensity.
        /// </summary>
        public int ILL_lx { get; set; }
    }
}
