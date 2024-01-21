using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeatherApp
{
    /// <summary>
    /// Represents sensor data in the WeatherApp.
    /// </summary>
    public class sensor_data
    {
        /// <summary>
        /// Gets or sets the unique data_id.
        /// </summary>
        [Key]
        public int data_id { get; set; }

        /// <summary>
        /// Gets or sets the device_id associated with the sensor data.
        /// </summary>
        public string device_id { get; set; }

        /// <summary>
        /// Gets or sets the temperature measured inside.
        /// </summary>
        public double? temperature_in { get; set; }

        /// <summary>
        /// Gets or sets the temperature measured outside.
        /// </summary>
        public double temperature_out { get; set; }

        /// <summary>
        /// Gets or sets the humidity. 
        /// </summary>
        public double humidity { get; set; }

        /// <summary>
        /// Gets or sets the ambient light intensity.
        /// </summary>
        public double ambient_light { get; set; }

        /// <summary>
        /// Gets or sets the barometric pressure.
        /// </summary>
        public double? barometric_pressure { get; set; }

        /// <summary>
        /// Gets or sets the date and time.
        /// </summary>
        public DateTime date_time { get; set; }
    }
}
