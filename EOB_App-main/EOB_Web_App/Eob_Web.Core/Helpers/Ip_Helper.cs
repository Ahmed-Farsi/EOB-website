using Eob_Web.Core.Models;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Eob_Web.Core.Helpers
{
    public class Ip_Range
    {
        public uint Start { get; set; }
        public uint End { get; set; }
    }

    public static class Ip_Helper
    {
        /// <summary>
        /// Convert ip as unsigned integer to a readable string
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string Uint_To_String(uint ip)
        {
            return string.Format("{0}.{1}.{2}.{3}", ip >> 24, (ip >> 16) & 0xff, (ip >> 8) & 0xff, ip & 0xff);
        }

        /// <summary>
        /// Convert CIDR notation to ip notation.
        /// Example: 24 -> 255.255.255.0
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static string Mask_To_Ip_Format(string mask)
        {
            int mask_Bits = Convert.ToInt32(mask);
            uint ip = 0xffffffff;
            ip <<= (32 - mask_Bits);
            return Uint_To_String(ip);
        }

        /// <summary>
        /// Check if ip is in the range of the specified subnet.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="subnet"></param>
        /// <returns></returns>
        public static bool Check_Ip_Address(string ip, string subnet)
        {
            if (ip == null)
                return true;

            string[] parts = subnet.Split('/');

            int ip_Address = BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes());
            int cidr_Address = BitConverter.ToInt32(IPAddress.Parse(parts[0]).GetAddressBytes());
            int cidr_Mask = IPAddress.HostToNetworkOrder(-1 << (32 - int.Parse(parts[1])));

            return ((ip_Address & cidr_Mask) == (cidr_Address & cidr_Mask));
        }

        /// <summary>
        /// Create a range from a subnet in CIDR notation. 
        /// Handy for creating for loops.
        /// </summary>
        /// <param name="subnet"></param>
        /// <returns></returns>
        public static Ip_Range Subnet_To_Range(string subnet)
        {
            string[] parts = subnet.Split('.', '/');

            uint ip = (Convert.ToUInt32(parts[0]) << 24) |
                      (Convert.ToUInt32(parts[1]) << 16) |
                      (Convert.ToUInt32(parts[2]) << 8) |
                      Convert.ToUInt32(parts[3]);

            int mask_Bits = Convert.ToInt32(parts[4]);
            uint mask = 0xffffffff;
            mask <<= (32 - mask_Bits);

            return new Ip_Range
            {
                Start = ip & mask,
                End = ip | ~mask
            };
        }
    }
}
