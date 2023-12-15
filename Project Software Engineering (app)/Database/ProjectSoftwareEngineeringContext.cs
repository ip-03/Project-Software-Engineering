using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Project_Software_Engineering.Database;

public partial class ProjectSoftwareEngineeringContext : DbContext
{
    public ProjectSoftwareEngineeringContext()
    {
    }

    public ProjectSoftwareEngineeringContext(DbContextOptions<ProjectSoftwareEngineeringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Gateway> Gateways { get; set; }

    public virtual DbSet<GatewayMetadatum> GatewayMetadata { get; set; }

    public virtual DbSet<SensorDatum> SensorData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=projectgroup5saxion.database.windows.net;Server=projectgroup5saxion.database.windows.net;Initial Catalog=project_software_engineering;Persist Security Info=False;User ID=project_group5_saxion;Password=weatherstation5!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.DeviceId).HasName("PK__device__3B085D8B353046B9");

            entity.ToTable("device");

            entity.Property(e => e.DeviceId)
                .HasMaxLength(32)
                .HasColumnName("device_id");
            entity.Property(e => e.BatteryStatus).HasColumnName("battery_status");
            entity.Property(e => e.GatewayId)
                .HasMaxLength(32)
                .HasColumnName("gateway_id");

            entity.HasOne(d => d.Gateway).WithMany(p => p.Devices)
                .HasForeignKey(d => d.GatewayId)
                .HasConstraintName("FK__device__gateway___01142BA1");
        });

        modelBuilder.Entity<Gateway>(entity =>
        {
            entity.HasKey(e => e.GatewayId).HasName("PK__gateway__0AF5B00B0C669A4C");

            entity.ToTable("gateway");

            entity.Property(e => e.GatewayId)
                .HasMaxLength(32)
                .HasColumnName("gateway_id");
            entity.Property(e => e.Altitude).HasColumnName("altitude");
            entity.Property(e => e.AvgAirtime).HasColumnName("avg_airtime");
            entity.Property(e => e.AvgRssi).HasColumnName("avg_rssi");
            entity.Property(e => e.AvgSnr).HasColumnName("avg_snr");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
        });

        modelBuilder.Entity<GatewayMetadatum>(entity =>
        {
            entity.HasKey(e => e.MetadataId).HasName("PK__gateway___C1088FC4ACD66CF3");

            entity.ToTable("gateway_metadata", tb => tb.HasTrigger("Update_"));

            entity.Property(e => e.MetadataId).HasColumnName("metadata_id");
            entity.Property(e => e.Airtime).HasColumnName("airtime");
            entity.Property(e => e.GatewayId)
                .HasMaxLength(32)
                .HasColumnName("gateway_id");
            entity.Property(e => e.Rssi).HasColumnName("rssi");
            entity.Property(e => e.Snr).HasColumnName("snr");

            entity.HasOne(d => d.Gateway).WithMany(p => p.GatewayMetadata)
                .HasForeignKey(d => d.GatewayId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__gateway_m__gatew__160F4887");
        });

        modelBuilder.Entity<SensorDatum>(entity =>
        {
            entity.HasKey(e => e.DataId).HasName("PK__sensor_d__F5A76B3BBABEC045");

            entity.ToTable("sensor_data");

            entity.Property(e => e.DataId).HasColumnName("data_id");
            entity.Property(e => e.AmbientLight).HasColumnName("ambient_light");
            entity.Property(e => e.BarometricPressure).HasColumnName("barometric_pressure");
            entity.Property(e => e.DateTime)
                .HasColumnType("datetime")
                .HasColumnName("date_time");
            entity.Property(e => e.DeviceId)
                .HasMaxLength(32)
                .HasColumnName("device_id");
            entity.Property(e => e.Humidity).HasColumnName("humidity");
            entity.Property(e => e.TemperatureIn).HasColumnName("temperature_in");
            entity.Property(e => e.TemperatureOut).HasColumnName("temperature_out");

            entity.HasOne(d => d.Device).WithMany(p => p.SensorData)
                .HasForeignKey(d => d.DeviceId)
                .HasConstraintName("FK__sensor_da__devic__06CD04F7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
