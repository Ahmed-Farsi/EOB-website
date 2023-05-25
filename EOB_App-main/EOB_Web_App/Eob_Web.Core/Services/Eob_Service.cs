using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eob_Web.Core.Models;

namespace Eob_Web.Core.Services
{
    public class Eob_Service : IEob_Service
    {
        private readonly IRepository _db;

        public Eob_Service(IRepository db)
        {
            _db = db;
        }

        // the mapping that we need to pass to dapper in order to join objects
        private static Eob Mapping(Eob eob, Company company, Group group, Subscription subscription)
        {
            eob.Company = company;
            eob.Group = group;
            eob.Subscription = subscription;
            return eob;
        }

        public async Task<List<Eob>> Get_All()
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_All,
                new { },
                Mapping);

            return result.ToList();
        }

        public async Task<Eob> Get_By_Id(int id)
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_By_Id,
                new { Id = id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<List<Eob>> Get_All_By_Company_Id(int company_Id)
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_All_By_Company_Id,
                new { Company_Id = company_Id },
                Mapping);

            return result.ToList();
        }

        public async Task<Eob> Get_By_Company_Id(int id, int company_Id)
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_By_Company_Id,
                new { Id = id, Company_Id = company_Id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<Eob> Get_By_Node_Id(string node_Id)
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_By_Node_Id,
                new { Node_Id = node_Id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<Eob> Get_By_Network_Id(string network_Id)
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_By_Network_Id,
                new { Network_Id = network_Id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<List<Eob>> Search(string term)
        {
            var result = await _db.Load_Multi<Eob, Company, Group, Subscription, Eob, dynamic>(
                Eob_Procedure.Get_All,
                new { },
                Mapping);

            if (string.IsNullOrEmpty(term))
                return result.ToList();

            return result.Where(x => x.Serial_Number.ToLower().Contains(term.ToLower()) ||
                                     x.Node_Name.ToLower().Contains(term.ToLower()) ||
                                     x.Company.Name.ToLower().Contains(term.ToLower()) ||
                                     x.Group_Id.HasValue && x.Group.Name.ToLower().Contains(term.ToLower()))
                .ToList();
        }

        public async Task<int> Create(Eob eob)
        {
            var data = new
            {
                eob.Id,
                eob.Serial_Number,
                eob.Network_Id,
                eob.Node_Id,
                eob.Node_Name,
                eob.Subscription_Id,
                eob.Company_Id,
                eob.Group_Id
            };

            return await _db.Modify<dynamic>(Eob_Procedure.Create, data);
        }

        public async Task<int> Update(Eob eob)
        {
            var data = new
            {
                eob.Id,
                eob.Serial_Number,
                eob.Network_Id,
                eob.Node_Id,
                eob.Node_Name,
                eob.Subscription_Id,
                eob.Company_Id,
                eob.Group_Id
            };

            return await _db.Modify<dynamic>(Eob_Procedure.Update, data);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _db.Modify<dynamic>(Eob_Procedure.Delete, new { Id = id });
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }
    }
}
