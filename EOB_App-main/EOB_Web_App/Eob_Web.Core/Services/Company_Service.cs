using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eob_Web.Core.Models;

namespace Eob_Web.Core.Services
{
    public class Company_Service : ICompany_Service
    {
        private readonly IRepository _db;

        public Company_Service(IRepository db)
        {
            _db = db;
        }

        public async Task<List<Company>> Get_All()
        {
            var result = await _db.Load<Company, dynamic>(Company_Procedure.Get_All, new { });
            return result.ToList();
        }

        public async Task<Company> Get_By_Id(int id)
        {
            var result = await _db.Load<Company, dynamic>(Company_Procedure.Get_By_Id, new { Id = id });
            return result.FirstOrDefault();
        }

        public async Task<Company> Get_By_Invite_Code(Guid invite_Code)
        {
            var result = await _db.Load<Company, dynamic>(Company_Procedure.Get_By_Invite_Code, new { Invite_Code = invite_Code });
            return result.FirstOrDefault();
        }

        public async Task<List<Company>> Search(string term)
        {
            var result = await _db.Load<Company, dynamic>(Company_Procedure.Get_All, new { });

            if (string.IsNullOrEmpty(term))
                return result.ToList();

            return result.Where(x => x.Name.ToLower().Contains(term.ToLower()) ||
                                     x.Address.ToLower().Contains(term.ToLower()) ||
                                     x.Email_Address.ToLower().Contains(term.ToLower()))
                .ToList();
        }

        public async Task<int> Create(Company company)
        {
            var data = new
            { 
                company.Id,
                company.Name,
                company.Address,
                company.Email_Address,
                company.Phone_Number,
                company.Invite_Code
            };

            return await _db.Modify<dynamic>(Company_Procedure.Create, data);
        }

        public async Task<int> Update(Company company)
        {
            var data = new
            { 
                company.Id,
                company.Name,
                company.Address,
                company.Email_Address,
                company.Phone_Number,
                company.Invite_Code
            };

            return await _db.Modify<dynamic>(Company_Procedure.Update, data);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _db.Modify<dynamic>(Company_Procedure.Delete, new { Id = id });
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }
    }
}
