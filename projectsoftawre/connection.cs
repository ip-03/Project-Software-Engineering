using Microsoft.EntityFrameworkCore;
using System;

namespace WeatherApp
{
    /// <summary>
    /// Represents the main database context for the WeatherApp.
    /// </summary>
    public class Main : DbContext
    {
        /// <summary>
        /// Gets or sets the DbSet representing the 'device' table.
        /// </summary>
        public DbSet<device> device { get; set; }

        /// <summary>
        /// Gets or sets the DbSet representing the 'gateway' table.
        /// </summary>
        public DbSet<gateway> gateway { get; set; }

        /// <summary>
        /// Gets or sets the DbSet representing the 'sensor_data' table.
        /// </summary>
        public DbSet<sensor_data> sensor_data { get; set; }

        /// <summary>
        /// Gets or sets the DbSet representing the 'gateway_metadata' table.
        /// </summary>
        public DbSet<gateway_metadata> gateway_metadata { get; set; }

        /// <summary>
        /// Configures the database connection settings.
        /// </summary>
        /// <param name="optionsBuilder">The options builder used for configuring the database context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=172.20.10.2;Initial Catalog=PSE;User ID=sa;Password=YourStrongPassword123!;Connection Timeout=30;";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
