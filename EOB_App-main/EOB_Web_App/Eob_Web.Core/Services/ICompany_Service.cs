using Eob_Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface ICompany_Service
    {
        Task<int> Create(Company company);
        Task<bool> Delete(int id);
        Task<List<Company>> Get_All();
        Task<Company> Get_By_Id(int id);
        Task<Company> Get_By_Invite_Code(Guid invite_Code);
        Task<List<Company>> Search(string term);
        Task<int> Update(Company company);
    }
}