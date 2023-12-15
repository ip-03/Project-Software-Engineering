using System;
using System.Collections.Generic;

namespace Project_Software_Engineering.Database;

public partial class SensorDatum
{
    public int DataId { get; set; }

    public string? DeviceId { get; set; }

    public double? TemperatureIn { get; set; }

    public double? TemperatureOut { get; set; }

    public double? Humidity { get; set; }

    public double? AmbientLight { get; set; }

    public double? BarometricPressure { get; set; }

    public DateTime? DateTime { get; set; }

    public virtual Device? Device { get; set; }
}
