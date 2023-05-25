using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;

namespace Utilities
{
    public static class Tools
    {
        private static void Run_Command(ProcessStartInfo psi)
        { 
            psi.CreateNoWindow = true;
            psi.UseShellExecute = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.Verb = "runas";

            var process = new Process();
            process.StartInfo = psi;
            process.Start();
            process.WaitForExit();
        }

        public static void Copy_File_With_Admin_Rights(string source_Path, string destination_Path)
        {
            var psi = new ProcessStartInfo("cmd.exe", $@"/C copy { source_Path } { destination_Path }");
            Run_Command(psi);
        }

        public static void ZT_Add_Network_Route(string network_Id, string ip, string mask)
        {
            var psi = new ProcessStartInfo("netsh", $"interface ipv4 add address name=\"ZeroTier One [{network_Id}]\" {ip} {mask}");
            Run_Command(psi);
        }
    }
}
