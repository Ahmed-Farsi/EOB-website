using Eob_Web.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public class ZeroTier_Service : IZeroTier_Service
    {
        private HttpClient _client;
        private IConfiguration _configuration;

        public ZeroTier_Service(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;

            string uri = _configuration.GetValue<string>($"Zerotier:Uri");
            string secret = _configuration.GetValue<string>($"Zerotier:Secret");

            _client.BaseAddress = new Uri(uri);
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {secret}");
        }

        public async Task<ZT_Network> Get_Network(string network_Id)
        {
            var response = await _client.GetAsync($"network/{network_Id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }
            
            return await response.Content.ReadAsAsync<ZT_Network>();
        }

        public async Task<ZT_Node> Get_Node(string network_Id, string node_Id)
        {
            var response = await _client.GetAsync($"network/{network_Id}/member/{node_Id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Node>();
        }

        public async Task<List<ZT_Node>> Get_All_Nodes(string network_Id)
        {
            var response = await _client.GetAsync($"network/{network_Id}/member");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }

            return await response.Content.ReadAsAsync<List<ZT_Node>>();
        }

        public async Task<ZT_Node> Update_Node(ZT_Node node)
        {
            var response = await _client.PostAsJsonAsync(
                $"network/{node.networkId}/member/{node.nodeId}",
                node);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Node>();
        }

        public async Task<bool> Delete_Node(string network_Id, string node_Id)
        {
            var response = await _client.DeleteAsync($"network/{network_Id}/member/{node_Id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return false;
            }

            return true;
        }

        public async Task<ZT_Network> Create_Network(ZT_Network network)
        {
            var response = await _client.PostAsJsonAsync($"network", network);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Network>();
        }

        public async Task<ZT_Network> Update_Network(ZT_Network network)
        {
            var response = await _client.PostAsJsonAsync($"network/{network.id}", network);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return null;
            }

            return await response.Content.ReadAsAsync<ZT_Network>();
        }

        public async Task<bool> Delete_Network(string network_Id)
        {
            var response = await _client.DeleteAsync($"network/{network_Id}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                return false;
            }

            return true; 
        }
    }
}
