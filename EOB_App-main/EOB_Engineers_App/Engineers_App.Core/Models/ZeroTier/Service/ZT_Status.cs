namespace Engineers_App.Core.Models.ZeroTier
{
    public class ZT_Status
    {
        public string address { get; set; }
        public long clock { get; set; }
        public Config config { get; set; }
        public bool online { get; set; }
        public long planetWorldId { get; set; }
        public long planetWorldTimestamp { get; set; }
        public string publicIdentity { get; set; }
        public bool tcpFallbackActive { get; set; }
        public string version { get; set; }
        public int versionBuild { get; set; }
        public int versionMajor { get; set; }
        public int versionMinor { get; set; }
        public int versionRev { get; set; }
    }

    public class Config
    {
        public Settings settings { get; set; }
    }

    public class Settings
    {
        public bool allowTcpFallbackRelay { get; set; }
        public bool portMappingEnabled { get; set; }
        public int primaryPort { get; set; }
    }
}
