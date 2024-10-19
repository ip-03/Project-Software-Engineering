using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class gateway_metadata
    {
        [Key]
        public int metadata_id { get; set; }
        public int rssi { get; set; }
        public double snr { get; set; }
        public double airtime { get; set; }
        public string gateway_id { get; set; }


    }

}
