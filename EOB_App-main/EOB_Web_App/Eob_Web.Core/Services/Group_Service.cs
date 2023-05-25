using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eob_Web.Core.Models;

namespace Eob_Web.Core.Services
{
    public class Group_Service : IGroup_Service
    {
        private readonly IRepository _db;

        public Group_Service(IRepository db)
        {
            _db = db;
        }

        // the mapping that we need to pass to dapper in order to join objects
        private static Group Mapping(Group group, Company company)
        {
            group.Company = company;
            return group;
        }

        public async Task<List<Group>> Get_All()
        {
            var result = await _db.Load_Multi<Group, Company, Group, dynamic>(
                Group_Procedure.Get_All,
                new { },
                Mapping);

            return result.ToList();
        }

        public async Task<Group> Get_By_Id(int id)
        {
            var result = await _db.Load_Multi<Group, Company, Group, dynamic>(
                Group_Procedure.Get_By_Id,
                new { Id = id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<List<Group>> Get_All_By_Company_Id(int company_Id)
        {
            var result = await _db.Load_Multi<Group, Company, Group, dynamic>(
                Group_Procedure.Get_All_By_Company_Id,
                new { Company_Id = company_Id },
                Mapping);

            return result.ToList();
        }

        public async Task<Group> Get_By_Company_Id(int id, int company_Id)
        {
            var result = await _db.Load_Multi<Group, Company, Group, dynamic>(
                Group_Procedure.Get_By_Company_Id,
                new { Id = id, Company_Id = company_Id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<List<Group>> Search(string term)
        {
            var result = await _db.Load_Multi<Group, Company, Group, dynamic>(
                Group_Procedure.Get_All,
                new { },
                Mapping);

            if (string.IsNullOrEmpty(term))
                return result.ToList();

            return result.Where(x => x.Name.ToLower().Contains(term.ToLower()) ||
                                     x.Department.ToLower().Contains(term.ToLower()))
                .ToList();
        }

        public async Task<int> Create(Group company_Group)
        {
            var data = new
            {
                company_Group.Id,
                company_Group.Name,
                company_Group.Department,
                company_Group.Company_Id
            };

            return await _db.Modify<dynamic>(Group_Procedure.Create, data);
        }

        public async Task<int> Update(Group company_Group)
        {
            var data = new
            {
                company_Group.Id,
                company_Group.Name,
                company_Group.Department,
                company_Group.Company_Id
            };

            return await _db.Modify<dynamic>(Group_Procedure.Update, data);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _db.Modify<dynamic>(Group_Procedure.Delete, new { Id = id });
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }
    }
}
