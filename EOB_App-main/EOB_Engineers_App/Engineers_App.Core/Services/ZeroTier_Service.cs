using Engineers_App.Core.Models.ZeroTier;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities;
using Utilities.Web_Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Services
{
    public class ZeroTier_Service : Web_Api, IZeroTier_Service
    {
        public ZeroTier_Service(IConfiguration config, ILogger<Web_Api> logger) : base(config, logger)
        {
            Initialize_Client("ZeroTier");
        }

        private string get_Auth_Token(string application_Path)
        {
            string auth_Token_Path = Path.Combine(application_Path, "authtoken.secret");

            if (!File.Exists(auth_Token_Path))
            {

                string zerotierFilePath = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                        "ZeroTier",
                        "One",
                        "authtoken.secret");

                Directory.CreateDirectory(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EOB"));

                Tools.Copy_File_With_Admin_Rights(zerotierFilePath, auth_Token_Path);
            }

            return File.ReadAllText(auth_Token_Path);
        }

        public void Add_Token(string application_Path)
        {
            string auth_Token = get_Auth_Token(application_Path);
            Client.DefaultRequestHeaders.Add("X-ZT1-Auth", auth_Token);
        }

        public async Task<ZT_Status> Get_Status()
        {
            using HttpResponseMessage response = await Client.GetAsync("status");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Status>();
        }

        public async Task<List<ZT_Network>> Get_Networks()
        {
            using HttpResponseMessage response = await Client.GetAsync("network");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<ZT_Network>>();
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public async Task<ZT_Network> Join_Network(string network_Id)
        {
            using HttpResponseMessage response = await Client.PostAsJsonAsync($"network/{network_Id}", "");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Network>();
        }

        public async Task<bool> Leave_Network(string network_Id)
        {
            using HttpResponseMessage response = await Client.DeleteAsync($"network/{network_Id}");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }

        public async Task<ZT_Network> Get_Network(string network_Id)
        {
            using HttpResponseMessage response = await Client.GetAsync($"network/{network_Id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Network>();
        }
    }
}
