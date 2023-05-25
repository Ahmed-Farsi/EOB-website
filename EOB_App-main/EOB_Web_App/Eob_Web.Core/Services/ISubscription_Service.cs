using Eob_Web.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface ISubscription_Service
    {
        Task<int> Create(Subscription subscription);
        Task<bool> Delete(int id);
        Task<List<Subscription>> Get_All();
        Task<List<Subscription>> Get_All_By_Company_Id(int company_Id);
        Task<Subscription> Get_By_Company_Id(int id, int company_Id);
        Task<Subscription> Get_By_Id(int id);
        Task<int> Update(Subscription subscription);
    }
}