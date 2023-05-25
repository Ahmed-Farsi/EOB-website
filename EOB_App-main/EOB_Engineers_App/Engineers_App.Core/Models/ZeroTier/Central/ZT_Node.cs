#pragma warning disable IDE1006 // Naming Styles
namespace Engineers_App.Core.Models.ZeroTier
{
    public class ZT_Node
    {
        public string id { get; set; }
        public string type { get; set; }
        public long clock { get; set; }
        public string networkId { get; set; }
        public string nodeId { get; set; }
        public string controllerId { get; set; }
        public bool hidden { get; set; }
        public string name { get; set; }
        public bool online { get; set; }
        public string description { get; set; }
        public ZT_Node_Config config { get; set; }
        public long lastOnline { get; set; }
        public string physicalAddress { get; set; }
        public object physicalLocation { get; set; }
        public string clientVersion { get; set; }
        public int protocolVersion { get; set; }
        public bool supportsRulesEngine { get; set; }
    }

    public class ZT_Node_Config
    {
        public bool activeBridge { get; set; }
        public string address { get; set; }
        public bool authorized { get; set; }
        public long creationTime { get; set; }
        public string id { get; set; }
        public string identity { get; set; }
        public string[] ipAssignments { get; set; }
        public long lastAuthorizedTime { get; set; }
        public long lastDeauthorizedTime { get; set; }
        public bool noAutoAssignIps { get; set; }
        public string nwid { get; set; }
        public int remoteTraceLevel { get; set; }
        public string remoteTraceTarget { get; set; }
        public int revision { get; set; }
        public int vMajor { get; set; }
        public int vMinor { get; set; }
        public int vRev { get; set; }
        public int vProto { get; set; }
        public bool ssoExempt { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles
