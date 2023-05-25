using Engineers_App.Core.Models.ZeroTier;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Engineers_App.Core.Services
{
    public interface IZeroTier_Service
    {
        void Add_Token(string application_Path);
        Task<ZT_Network> Get_Network(string network_Id);
        Task<List<ZT_Network>> Get_Networks();
        Task<ZT_Status> Get_Status();
        Task<ZT_Network> Join_Network(string network_Id);
        Task<bool> Leave_Network(string network_Id);
    }
}