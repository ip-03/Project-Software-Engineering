using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class gateway
    {
        [Key]
        public string gateway_id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int altitude { get; set; }
        public double avg_rssi { get; set; }
        public double avg_snr { get; set; }

        public double avg_airtime { get; set; }

    }
}
