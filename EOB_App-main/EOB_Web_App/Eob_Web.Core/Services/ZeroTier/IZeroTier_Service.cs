using Eob_Web.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IZeroTier_Service
    {
        Task<bool> Delete_Node(string network_Id, string node_Id);
        Task<ZT_Node> Get_Node(string network_Id, string node_Id);
        Task<List<ZT_Node>> Get_All_Nodes(string network_Id);
        Task<ZT_Node> Update_Node(ZT_Node node);
        Task<ZT_Network> Get_Network(string network_Id);
        Task<ZT_Network> Create_Network(ZT_Network node);
        Task<bool> Delete_Network(string network_Id);
        Task<ZT_Network> Update_Network(ZT_Network network);
    }
}