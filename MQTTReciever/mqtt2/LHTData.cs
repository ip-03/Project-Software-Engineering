using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt2
{
    internal class LHTData
    {
        public EndDeviceIds end_device_ids { get; set; }
        public DateTime received_at { get; set; }
        public LHTUplinkMessage uplink_message { get; set; }
    }

    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int? altitude { get; set; }
    }

    public class GatewayIds
    {
        public string gateway_id { get; set; }
    }

    public class RxMetadata
    {
        public GatewayIds gateway_ids { get; set; }
        public int rssi { get; set; }
        public double snr { get; set; }
        public Location location { get; set; }
        public DateTime received_at { get; set; }
    }
    public class EndDeviceIds
    {
        public string device_id { get; set; }
    }

    public class LHTUplinkMessage
    {
        public LHTDecodedPayload decoded_payload { get; set; }
        public List<RxMetadata> rx_metadata { get; set; }
        public string consumed_airtime { get; set; }
    }

    public class LHTDecodedPayload
    {
        public double BatV { get; set; }
        public int Bat_status { get; set; }
        public double TempC_SHT { get; set; }
        public double? TempC_DS { get; set; }
        public double Hum_SHT { get; set; }
        public int ILL_lx { get; set; }
    }
}