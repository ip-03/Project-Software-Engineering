using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp
{
    /// <summary>
    /// Represents metadata associated with a gateway in the WeatherApp application.
    /// </summary>
    public class gateway_metadata
    {
        /// <summary>
        /// Gets or sets the unique metadata_id.
        /// </summary>
        [Key]
        public int metadata_id { get; set; }

        /// <summary>
        /// Gets or sets the rssi value associated with the metadata.
        /// </summary>
        public int rssi { get; set; }

        /// <summary>
        /// Gets or sets the snr value associated with the metadata.
        /// </summary>
        public double snr { get; set; }

        /// <summary>
        /// Gets or sets the airtime value associated with the metadata.
        /// </summary>
        public double airtime { get; set; }

        /// <summary>
        /// Gets or sets the gateway_id associated with this metadata.
        /// </summary>
        public string gateway_id { get; set; }
    }
}
