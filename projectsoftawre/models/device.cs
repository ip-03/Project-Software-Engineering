using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp
{
    /// <summary>
    /// Represents the devices in the WeatherApp.
    /// </summary>
    public class device
    {
        /// <summary>
        /// Gets or sets the unique device_id.
        /// </summary>
        [Key]
        public string device_id { get; set; }

        /// <summary>
        /// Gets or sets the gateway_id associated with the device.
        /// </summary>
        public string gateway_id { get; set; }

        /// <summary>
        /// Gets or sets the battery_status of the device.
        /// </summary>
        public int? battery_status { get; set; }

        /// <summary>
        /// Gets or sets the BatV of the device.
        /// </summary>
        public double? BatV { get; set; }
    }
}
