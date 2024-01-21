using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt2
{
    /// <summary>
    /// Represents the data received from an MKR device.
    /// </summary>
    internal class MKRData
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
        /// Gets or sets the uplink message from the MKR device.
        /// </summary>
        public MKRUplinkMessage uplink_message { get; set; }
    }

    /// <summary>
    /// Represents an uplink message from an MKR device.
    /// </summary>
    public class MKRUplinkMessage
    {
        /// <summary>
        /// Gets or sets the decoded payload of the MKR uplink message.
        /// </summary>
        public MKRDecodedPayload decoded_payload { get; set; }

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
    /// Represents the decoded payload of an MKR uplink message.
    /// </summary>
    public class MKRDecodedPayload
    {
        /// <summary>
        /// Gets or sets the temperature reported by the MKR device.
        /// </summary>
        public double temperature { get; set; }

        /// <summary>
        /// Gets or sets the humidity reported by the MKR device.
        /// </summary>
        public double humidity { get; set; }

        /// <summary>
        /// Gets or sets the light intensity reported by the MKR device.
        /// </summary>
        public int light { get; set; }

        /// <summary>
        /// Gets or sets the atmospheric pressure reported by the MKR device.
        /// </summary>
        public int pressure { get; set; }
    }
}
