using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class device

    {
        [Key]
        public string device_id { get; set; }
        public string gateway_id { get; set; }
        public int battery_status { get; set; }
        public double BatV { get; set; }

    }

}
