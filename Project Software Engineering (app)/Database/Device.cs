using System;
using System.Collections.Generic;

namespace Project_Software_Engineering.Database;

public partial class Device
{
    public string DeviceId { get; set; } = null!;

    public string? GatewayId { get; set; }

    public int? BatteryStatus { get; set; }

    public double? BatV { get; set; }

    public virtual Gateway? Gateway { get; set; }

    public virtual ICollection<SensorDatum> SensorData { get; set; } = new List<SensorDatum>();
}
