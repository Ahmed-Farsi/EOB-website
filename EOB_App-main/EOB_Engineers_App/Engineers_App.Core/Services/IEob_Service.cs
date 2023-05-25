using Engineers_App.Core.Models;
using Engineers_App.Core.Models.ZeroTier;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Engineers_App.Core.Services
{
    public interface IEob_Service
    {
        HttpStatusCode Status_Code { get; }

        Task<List<Eob>> Get_All_Eobs();
        Task<User> Get_User(string token);
        Task<Auth_Result> Login(string Email_Address, string Password);
        Task<bool> ZT_Add_Subnet(string Network_Id, string subnet, string mask);
        Task<bool> ZT_Delete_Subnet(string Network_Id, string subnet, string mask);
        Task<ZT_Node> ZT_Get_Node(string Network_Id, string node_Id);
        Task<bool> ZT_Node_Assign(string Network_Id, string node_Id);
        Task<bool> ZT_Node_Reset(string Network_Id, string node_Id);
    }
}