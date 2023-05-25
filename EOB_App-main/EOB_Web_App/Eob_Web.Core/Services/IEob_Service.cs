using Eob_Web.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IEob_Service
    {
        Task<int> Create(Eob eob);
        Task<bool> Delete(int id);
        Task<Eob> Get_By_Node_Id(string node_Id);
        Task<List<Eob>> Get_All();
        Task<List<Eob>> Get_All_By_Company_Id(int company_Id);
        Task<Eob> Get_By_Company_Id(int id, int company_Id);
        Task<Eob> Get_By_Id(int id);
        Task<int> Update(Eob eob);
        Task<Eob> Get_By_Network_Id(string network_Id);
        Task<List<Eob>> Search(string term);
    }
}