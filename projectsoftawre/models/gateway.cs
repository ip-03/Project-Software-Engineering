using System.ComponentModel.DataAnnotations;

namespace WeatherApp
{
    /// <summary>
    /// Represents a gateway in the WeatherApp application.
    /// </summary>
    public class gateway
    {
        /// <summary>
        /// Gets or sets the unique gateway_id.
        /// </summary>
        [Key]
        public string gateway_id { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the gateway's location.
        /// </summary>
        public double latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the gateway's location.
        /// </summary>
        public double longitude { get; set; }

        /// <summary>
        /// Gets or sets the altitude of the gateway's location.
        /// </summary>
        public int? altitude { get; set; }

        /// <summary>
        /// Gets or sets the avg_rssi of the gateway.
        /// </summary>
        public double avg_rssi { get; set; }

        /// <summary>
        /// Gets or sets the avg_snr of the gateway.
        /// </summary>
        public double avg_snr { get; set; }

        /// <summary>
        /// Gets or sets the avg_airtime of the gateway.
        /// </summary>
        public double avg_airtime { get; set; }
    }
}
