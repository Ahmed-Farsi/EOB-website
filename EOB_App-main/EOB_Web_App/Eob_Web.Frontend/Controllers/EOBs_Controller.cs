using Eob_Web.Core.Models;
using Eob_Web.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mime;
using Eob_Web.Core;
using Eob_Web.Core.Helpers;
using System.Text.RegularExpressions;

namespace Eob_Web.Frontend.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EobsController : ControllerBase
    {
        private const int NETWORK_ROOM_LIMIT = 5;

        private IEob_Service _eob_Service;
        private IZeroTier_Service _zeroTier_Service;
        private IUser_Service _user_Service;
        private IHttpContextAccessor _http_Context_Accessor;
        private IWebHostEnvironment _env;

        private int _user_Id;

        private class ReturnMessage
        {
            public string status { get; set; } = "failed";
            public string message { get; set; } = "unkown";
        }

        private class UserReturnMessage
        {
            public string status { get; set; } = "failed";
            public string message { get; set; } = "unkown";
            public User result { get; set; }
        }

        private class DictReturnMessage
        {
            public string status { get; set; } = "failed";
            public string message { get; set; } = "unkown";
            public Dictionary<string, string> result { get; set; }
        }

        public EobsController(
            IEob_Service eob_Service,
            IZeroTier_Service zeroTier_Service,
            IUser_Service user_Service,
            IHttpContextAccessor http_Context_Accessor,
            IWebHostEnvironment env)
        {

            _eob_Service = eob_Service;
            _zeroTier_Service = zeroTier_Service;
            _user_Service = user_Service;
            _http_Context_Accessor = http_Context_Accessor;
            _env = env;

            if (_http_Context_Accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                bool res = int.TryParse(_http_Context_Accessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User_Id").Value, out _user_Id);

                if (!res)
                    return;
            }
        }

        private bool Security_Check(User user, Eob eob)
        {
            if (eob == null)
                return false;

            if (user.Role_Name != User_Roles.Admin && user.Company_Id != eob.Company_Id)
                return false; 

            if (user.Role_Name == User_Roles.Employee && !user.Groups.Any(x => x.Id == eob.Group_Id))
                return false;

            return true;
        }

        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet]
        public async Task<ActionResult<List<Eob>>> Get()
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eobs = new List<Eob>();

            if (user.Role_Name == User_Roles.Admin)
            {
                eobs = await _eob_Service.Get_All();
            }
            else
            { 
                eobs = await _eob_Service.Get_All_By_Company_Id(user.Company_Id.Value);
                if (user.Role_Name == User_Roles.Employee)
                    eobs.RemoveAll(x => !user.Groups.Any(y => y.Id == x.Group_Id));
            }

            return Ok(eobs);
        }

        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Eob>> Get(int id)
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eob = new Eob();

            if (user.Role_Name == User_Roles.Admin)
            { 
                eob = await _eob_Service.Get_By_Id(id);
            }
            else
            { 
                eob = await _eob_Service.Get_By_Company_Id(id, user.Company_Id.Value);

                if (!user.Groups.Any(x => x.Id == eob.Group_Id))
                    return Forbid();
            }

            if (eob == null)
                return NotFound();

            return Ok(eob);
        }

        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet("zt_node_assign/{network_Id}/{node_Id}")]
        public async Task<ActionResult> ZT_Node_Assign(string network_Id, string node_Id)
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eob = await _eob_Service.Get_By_Network_Id(network_Id);

            if (!Security_Check(user, eob))
                return Forbid();

            var network = await _zeroTier_Service.Get_Network(eob.Network_Id);
            var nodes = await _zeroTier_Service.Get_All_Nodes(eob.Network_Id);
            var current_Node = nodes.Find(x => x.nodeId == node_Id);

            // incase of deleted node (BTW, don't delete nodes off of the ZT network)
            if (current_Node == null)
            {
                for (int i = 0; i < 10; i++)
                { 
                    if (i == 10)
                        return BadRequest();

                    current_Node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);
                    if (current_Node != null)
                        break;

                    await Task.Delay(2000);
                }
            }

            int connections = 0;
            foreach (var node in nodes)
            {
                // (failsafe) remove extra ip addresses in case a node is offline
                if (!node.online && node.config.ipAssignments.Length > 1)
                {
                    string ip = Array.Find(node.config.ipAssignments, x => x.Contains("192.168.99"));
                    node.config.ipAssignments = new string[] { ip };
                    node.config.authorized = false;

                    await _zeroTier_Service.Update_Node(node);
                }

                if (node.config.authorized)
                    connections++;
            }

            // network room limit: 5 people excluding EOB. Why would you go higher anyway?
            if (connections > NETWORK_ROOM_LIMIT + 1)
                return Forbid("Network is full");

            // authorize new node first to auto assign ip
            if (!current_Node.config.ipAssignments.Any())
            { 
                current_Node.config.authorized = true;
                current_Node.name = user.Email_Address;
                await _zeroTier_Service.Update_Node(current_Node);

                for (int i = 0; i < 10; i++)
                { 
                    if (i == 10)
                        return BadRequest();

                    current_Node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);
                    if (current_Node.config.ipAssignments.Any())
                        break;

                    await Task.Delay(2000);
                }
            }

            foreach (var route in network.config.routes)
            {
                if (route.target == "192.168.99.0/24")
                    continue;

                var range = Ip_Helper.Subnet_To_Range(route.target);

                for (uint i = 0; i < NETWORK_ROOM_LIMIT; i++)
                {
                    string ip = Ip_Helper.Uint_To_String(range.End - (i + 1));
                    if (nodes.Any(node => node.config.ipAssignments.Any(x => x == ip)))
                        continue;

                    bool ip_Exists = false;

                    foreach (string assignment in current_Node.config.ipAssignments)
                    {
                        if (Ip_Helper.Check_Ip_Address(assignment, route.target))
                        { 
                            ip_Exists = true;
                            break;
                        }
                    }

                    if (ip_Exists)
                        break;

                    var ip_Assignments = current_Node.config.ipAssignments;
                    Array.Resize(ref ip_Assignments, ip_Assignments.Length + 1);
                    ip_Assignments[ip_Assignments.Length - 1] = ip;
                    current_Node.config.ipAssignments = ip_Assignments;

                    break;
                }
            }

            current_Node.config.authorized = true;
            current_Node.name = user.Email_Address;
            await _zeroTier_Service.Update_Node(current_Node);

            return Ok();
        }

        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet("zt_node_reset/{network_Id}/{node_Id}")]
        public async Task<ActionResult> ZT_Node_Reset(string network_Id, string node_Id)
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eob = await _eob_Service.Get_By_Network_Id(network_Id);

            if (!Security_Check(user, eob))
                return Forbid();

            var node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);

            string ip = Array.Find(node.config.ipAssignments, x => x.Contains("192.168.99"));
            node.config.ipAssignments = new string[] { ip };
            node.config.authorized = false;

            await _zeroTier_Service.Update_Node(node);

            return Ok();
        }


        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet("zt_add_subnet/{network_Id}/{subnet}/{mask}")]
        public async Task<ActionResult> ZT_Add_Subnet(string network_Id, string subnet, string mask)
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eob = await _eob_Service.Get_By_Network_Id(network_Id);

            if (!Security_Check(user, eob))
                return Forbid();

            var network = await _zeroTier_Service.Get_Network(eob.Network_Id);
            var routes = network.config.routes;
            Array.Resize(ref routes, routes.Length + 1);
            routes[routes.Length - 1] = new Routes { target = string.Join('/', subnet, mask) };
            network.config.routes = routes;
            await _zeroTier_Service.Update_Network(network);

            return Ok();
        }

        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet("zt_delete_subnet/{network_Id}/{subnet}/{mask}")]
        public async Task<ActionResult> ZT_Delete_Subnet(string network_Id, string subnet, string mask)
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eob = await _eob_Service.Get_By_Network_Id(network_Id);

            if (!Security_Check(user, eob))
                return Forbid();

            var network = await _zeroTier_Service.Get_Network(eob.Network_Id);
            int index = Array.FindIndex(network.config.routes, x => x.target == string.Join('/', subnet, mask));
            network.config.routes = network.config.routes
                .Where((source, i) => i != index)
                .ToArray();

            await _zeroTier_Service.Update_Network(network);

            return Ok();
        }

        [Authorize(Roles = User_Roles.Humans)]
        [HttpGet("zt_get_node/{network_Id}/{node_Id}")]
        public async Task<ActionResult<ZT_Node>> ZT_Get_Node(string network_Id, string node_Id)
        {
            var user = await _user_Service.Get_By_Id(_user_Id);
            var eob = await _eob_Service.Get_By_Network_Id(network_Id);

            if (!Security_Check(user, eob))
                return Forbid();

            var node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);
            if (node == null)
                return NotFound();

            return Ok(node);
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public ActionResult Version()
        {
            string path = Path.Combine(_env.WebRootPath, "files", "EOB");
            string file = Directory.GetFiles(path).Last();
            string version = file.Substring(file.LastIndexOf(@"/")+1).Replace(".zip", "");

            return Ok(new ReturnMessage
            {
                status = "success",
                message = version
            });
        }

        [HttpGet("download/{version}")]
        [AllowAnonymous]
        public ActionResult Download(string version)
        {
            //TODO: add new blazor page to add files to the server
            string path = Path.Combine(_env.WebRootPath, "files", "EOB");
            byte[] fileBytes = System.IO.File.ReadAllBytes(Path.Combine(path, $"{version}.zip"));
            return File(fileBytes, MediaTypeNames.Application.Octet, $"{version}.zip");
        }

        [HttpPost("register_eob")]
        public ActionResult RegisterEob(Eob eob)
        {
            return Ok(eob);
        }

        [HttpGet("enable_maintenance_mode/{node_Id}")]
        public async Task<ActionResult> Enable_Maintenance_Mode(string node_Id)
        {
            var eob = await _eob_Service.Get_By_Node_Id(node_Id);
            if (eob == null)
                return NotFound();

            ZT_Node node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);

            node.config.authorized = true;

            await _zeroTier_Service.Update_Node(node);
            await _eob_Service.Update(eob);

            var msg = new ReturnMessage()
            {
                status = "success",
                message = "successfully set eob to maintenance mode"
            };

            return Ok(msg);
        }

        [HttpGet("disable_maintenance_mode/{node_Id}")]
        public async Task<ActionResult> Disable_Maintenance_Mode(string node_Id)
        {
            var eob = await _eob_Service.Get_By_Node_Id(node_Id);
            if (eob == null)
                return NotFound();

            var node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);

            node.config.authorized = false;

            await _zeroTier_Service.Update_Node(node);
            await _eob_Service.Update(eob);

            var msg = new ReturnMessage()
            {
                status = "success",
                message = "successfully set eob to maintenance mode"
            };

            return Ok(msg);
        }

        //TODO: change to "connect_node"
        [HttpGet("connect_member/{node_Id}")]
        public async Task<ActionResult> Connect_Member(string node_Id)
        {
            var eob = await _eob_Service.Get_By_Node_Id(node_Id);
            if (eob == null)
                return NotFound();

            var node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);
            node.config.authorized = true;
            await _zeroTier_Service.Update_Node(node);

            var msg = new ReturnMessage()
            {
                status = "success",
                message = "successfully opened the connection",
            };

            return Ok(msg);
        }

        //TODO: change to "connect_node"
        [HttpGet("disconnect_member/{node_Id}")]
        public async Task<ActionResult> Disconnect_Member(string node_Id)
        {
            var eob = await _eob_Service.Get_By_Node_Id(node_Id);
            if (eob == null)
                return NotFound();

            var node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);
            node.config.authorized = false;
            await _zeroTier_Service.Update_Node(node);

            var msg = new ReturnMessage()
            {
                status = "success",
                message = "successfully closed the connection",
            };

            return Ok(msg);
        }

        [HttpGet("update_request/{node_Id}")]
        public async Task<ActionResult> Update_Request(string node_Id)
        {
            var eob = await _eob_Service.Get_By_Node_Id(node_Id);
            var node = await _zeroTier_Service.Get_Node(eob.Network_Id, node_Id);

            if (node.config.authorized)
            {
                var nodes = await _zeroTier_Service.Get_All_Nodes(eob.Network_Id);
                ZT_Node networkUser = null;

                for (int i = 0; i < nodes.Count; i++)
                    if (nodes[i].config.authorized && nodes[i].name != eob.Node_Name && nodes[i].online)
                        networkUser = nodes[i];

                if (networkUser == null)
                {
                    var dictReturnMessage = new DictReturnMessage()
                    {
                        status = "success",
                        message = "Disconnected",
                        result = new Dictionary<string, string>() {
                        {"companyName", eob.Company.Name},
                        {"engineerName", ""},
                    },
                    };
                    return Ok(dictReturnMessage);
                }

                var msg = new DictReturnMessage()
                {
                    status = "success",
                    message = "returned the current connections",
                    result = new Dictionary<string, string>() {
                        {"companyName", eob.Company.Name},
                        {"engineerName", networkUser.name},
                    },

                };
                return Ok(msg);
            }
            else
            {
                var msg = new DictReturnMessage()
                {
                    status = "success",
                    message = "Disconnected",
                    result = new Dictionary<string, string>() {
                        {"companyName", eob.Company.Name},
                        {"engineerName", ""},
                    },
                };
                return Ok(msg);
            }
        }
    }
}
