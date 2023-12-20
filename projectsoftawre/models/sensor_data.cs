using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class sensor_data
    {
        [Key]
        public int data_id { get; set; }
        public string device_id { get; set; }
        public double? temperature_in { get; set; }  // Change to double
        public double temperature_out { get; set; }  // Change to double

        public double humidity { get; set; }  // Change to double
        public double ambient_light { get; set; }  // Change to double
        public double? barometric_pressure { get; set; }  // Change to double
        public DateTime date_time { get; set; }
    }

}
