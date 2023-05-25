using System.Runtime.Serialization;

#pragma warning disable IDE1006 // Naming Styles
namespace Eob_Web.Core.Models
{
    public class ZT_Network
    {
        public string id { get; set; }
        public long clock { get; set; }
        public ZT_Network_Config config { get; set; }
        public string description { get; set; }
        public string rulesSource { get; set; }
        public string ownerId { get; set; }
        public int onlineMemberCount { get; set; }
        public int authorizedMemberCount { get; set; }
        public int totalMemberCount { get; set; }
    }

    [DataContract]
    public class ZT_Network_Config
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public long creationTime { get; set; }
        [DataMember]
        public bool enableBroadcast { get; set; }
        [DataMember]
        public IpAssignmentPools[] ipAssignmentPools { get; set; }
        [DataMember]
        public long lastModified { get; set; }
        [DataMember]
        public int mtu { get; set; }
        [DataMember]
        public int multicastLimit { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember(Name = "private")] // reserved keyword
        public bool __private { get; set; } = true;
        [DataMember]
        public Routes[] routes { get; set; }
        [DataMember]
        public v6AssignMode v6AssignMode { get; set; } = new();
    }

    public class IpAssignmentPools
    {
        public string ipRangeStart { get; set; }
        public string ipRangeEnd { get; set; }
    }

    public class Routes
    {
        public string target { get; set; }
        public string via { get; set; }
    }

    [DataContract]
    public class v6AssignMode
    {
        [DataMember(Name = "6plane")] // reserved keyword
        public bool __6plane { get; set; } = false;
    }
}
#pragma warning restore IDE1006 // Naming Styles
