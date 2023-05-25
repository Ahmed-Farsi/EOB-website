using Engineers_App.Core.Models;
using Engineers_App.Core.Models.ZeroTier;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Utilities;
using Utilities.Web_Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Services
{
    public class Eob_Service : Web_Api, IEob_Service
    {
        public HttpStatusCode Status_Code { get; private set; }

        public Eob_Service(IConfiguration config, ILogger<Web_Api> logger) : base(config, logger)
        {
            Initialize_Client("Eob");
        }

        public async Task<Auth_Result> Login(string email_Address, string password)
        {
            var login = new Login
            {
                Email_Address = email_Address,
                Password = password
            };

            using HttpResponseMessage response = await Client.PostAsJsonAsync("authorization", login);
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return null;
            }

            var result = await response.Content.ReadAsAsync<Auth_Result>();

            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", result.Token);

            return result;
        }

        public async Task<User> Get_User(string token)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            using HttpResponseMessage response = await Client.GetAsync("authorization/get_user");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                Client.DefaultRequestHeaders.Authorization = null;
                return null;
            }

            var result = await response.Content.ReadAsAsync<User>();

            return result;
        }

        public async Task<List<Eob>> Get_All_Eobs()
        {
            using HttpResponseMessage response = await Client.GetAsync("eobs");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return null;
            }

            return await response.Content.ReadAsAsync<List<Eob>>();
        }

        public async Task<ZT_Node> ZT_Get_Node(string network_Id, string node_Id)
        {
            using HttpResponseMessage response = await Client.GetAsync($"eobs/zt_get_node/{network_Id}/{node_Id}");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Node>();
        }

        public async Task<bool> ZT_Node_Assign(string network_Id, string node_Id)
        {
            using HttpResponseMessage response = await Client.GetAsync($"eobs/zt_node_assign/{network_Id}/{node_Id}");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return false;
            }

            return true;
        }

        public async Task<bool> ZT_Node_Reset(string network_Id, string node_Id)
        {
            using HttpResponseMessage response = await Client.GetAsync($"eobs/zt_node_reset/{network_Id}/{node_Id}");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return false;
            }

            return true;
        }

        public async Task<bool> ZT_Add_Subnet(string network_Id, string subnet, string mask)
        {
            using HttpResponseMessage response = await Client.GetAsync($"eobs/zt_add_subnet/{network_Id}/{subnet}/{mask}");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return false;
            }

            return true;
        }

        public async Task<bool> ZT_Delete_Subnet(string network_Id, string subnet, string mask)
        {
            using HttpResponseMessage response = await Client.GetAsync($"eobs/zt_delete_subnet/{network_Id}/{subnet}/{mask}");
            if (!response.IsSuccessStatusCode)
            {
                Status_Code = response.StatusCode;
                return false;
            }

            return true;
        }
    }
}
