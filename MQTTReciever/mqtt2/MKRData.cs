using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mqtt2
{
    internal class MKRData
    {
        public EndDeviceIds end_device_ids { get; set; }
        public DateTime received_at { get; set; }
        public MKRUplinkMessage uplink_message { get; set; }
    }

    public class MKRUplinkMessage
    {
        public MKRDecodedPayload decoded_payload { get; set; }
        public List<RxMetadata> rx_metadata { get; set; }
        public string consumed_airtime { get; set; }
    }
    public class MKRDecodedPayload
    {
        public double temperature { get; set; }
        public double humidity { get; set; }
        public int light { get; set; }
        public int pressure { get; set; }
    }
}
