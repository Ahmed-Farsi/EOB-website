using Eob_Web.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IGroup_Service
    {
        Task<int> Create(Group company_Group);
        Task<bool> Delete(int id);
        Task<List<Group>> Get_All();
        Task<List<Group>> Get_All_By_Company_Id(int company_Id);
        Task<Group> Get_By_Company_Id(int id, int company_Id);
        Task<Group> Get_By_Id(int id);
        Task<List<Group>> Search(string term);
        Task<int> Update(Group company_Group);
    }
}