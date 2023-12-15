using System;
using System.Collections.Generic;

namespace Project_Software_Engineering.Database;

public partial class Gateway
{
    public string GatewayId { get; set; } = null!;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int? Altitude { get; set; }

    public double? AvgRssi { get; set; }

    public double? AvgSnr { get; set; }

    public double? AvgAirtime { get; set; }

    public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

    public virtual ICollection<GatewayMetadatum> GatewayMetadata { get; set; } = new List<GatewayMetadatum>();
}
