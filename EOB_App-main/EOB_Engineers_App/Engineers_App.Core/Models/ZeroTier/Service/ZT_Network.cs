namespace Engineers_App.Core.Models.ZeroTier
{
    public class ZT_Network
    {
        public bool allowDNS { get; set; }
        public bool allowDefault { get; set; }
        public bool allowGlobal { get; set; }
        public bool allowManaged { get; set; }
        public string[] assignedAddresses { get; set; }
        public bool bridge { get; set; }
        public bool broadcastEnabled { get; set; }
        public Dns dns { get; set; }
        public string id { get; set; }
        public string mac { get; set; }
        public int mtu { get; set; }
        public Multicastsubscription[] multicastSubscriptions { get; set; }
        public string name { get; set; }
        public int netconfRevision { get; set; }
        public string portDeviceName { get; set; }
        public int portError { get; set; }
        public Route[] routes { get; set; }
        public string status { get; set; }
        public string type { get; set; }
    }

    public class Dns
    {
        public string domain { get; set; }
        public string[] servers { get; set; }
    }

    public class Multicastsubscription
    {
        public uint adi { get; set; }
        public string mac { get; set; }
    }

    public class Route
    {
        public int flags { get; set; }
        public int metric { get; set; }
        public string target { get; set; }
        public string via { get; set; }
    }
}
