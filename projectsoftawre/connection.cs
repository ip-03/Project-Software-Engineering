using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp;

namespace WeatherApp
{
    public class Main : DbContext
    {
        public DbSet<device> device { get; set; } // represent the tables
        public DbSet<gateway> gateway { get; set; }
        public DbSet<sensor_data> sensor_data { get; set; }
        public DbSet<gateway_metadata> gateway_metadata { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=172.20.10.2;Initial Catalog=PSE;User ID=sa;Password=YourStrongPassword123!;Connection Timeout=30;";


            optionsBuilder.UseSqlServer(connectionString); // connect to the database
        }
    }

}
