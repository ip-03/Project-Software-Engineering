using System;
using System.Collections.Generic;

namespace Project_Software_Engineering.Database;

public partial class GatewayMetadatum
{
    public int MetadataId { get; set; }

    public int Rssi { get; set; }

    public double Snr { get; set; }

    public double Airtime { get; set; }

    public string GatewayId { get; set; } = null!;

    public virtual Gateway Gateway { get; set; } = null!;
}
