using System.Collections.Generic;

namespace Engineers_App.Core.Models.ZeroTier
{
    public class ZT_Peer_Status
    {
        public string address { get; set; }
        public bool isBonded { get; set; }
        public int latency { get; set; }
        public List<Peer_Path> paths { get; set; }
        public string role { get; set; }
        public string version { get; set; }
        public int versionMajor { get; set; }
        public int versionMinor { get; set; }
        public int versionRev { get; set; }
    }

    public class Peer_Path
    {
        public bool active { get; set; }
        public string address { get; set; }
        public bool expired { get; set; }
        public long lastReceive { get; set; }
        public long lastSend { get; set; }
        public bool preferred { get; set; }
        public int trustedPathId { get; set; }
    }

}
