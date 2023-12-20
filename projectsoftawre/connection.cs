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
            string connectionString = "Server=projectgroup5saxion.database.windows.net;Initial Catalog=project_software_engineering;Persist Security Info=False;User ID=project_group5_saxion;Password=weatherstation5!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString); // connect to the database
        }
    }

}
