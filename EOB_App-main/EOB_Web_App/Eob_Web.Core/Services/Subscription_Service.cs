using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eob_Web.Core.Models;

namespace Eob_Web.Core.Services
{
    public class Subscription_Service : ISubscription_Service
    {
        private readonly IRepository _db;

        public Subscription_Service(IRepository db)
        {
            _db = db;
        }

        private static Subscription Mapping(Subscription subscription, Company company)
        {
            subscription.Company = company;
            return subscription;
        }

        public async Task<List<Subscription>> Get_All()
        {
            var result = await _db.Load_Multi<Subscription, Company, Subscription, dynamic>(
                Subscription_Procedure.Get_All,
                new { },
                Mapping);

            return result.ToList();
        }

        public async Task<Subscription> Get_By_Id(int id)
        {
            var result = await _db.Load_Multi<Subscription, Company, Subscription, dynamic>(
                Subscription_Procedure.Get_By_Id,
                new { Id = id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<List<Subscription>> Get_All_By_Company_Id(int company_Id)
        {
            var result = await _db.Load_Multi<Subscription, Company, Subscription, dynamic>(
                Subscription_Procedure.Get_All_By_Company_Id,
                new { Company_Id = company_Id },
                Mapping);

            return result.ToList();
        }

        public async Task<Subscription> Get_By_Company_Id(int id, int company_Id)
        {
            var result = await _db.Load_Multi<Subscription, Company, Subscription, dynamic>(
                Subscription_Procedure.Get_By_Company_Id,
                new { Id = id, Company_Id = company_Id },
                Mapping);

            return result.FirstOrDefault();
        }

        public async Task<int> Create(Subscription subscription)
        {
            var data = new
            {
                subscription.Id,
                subscription.Start_Date,
                subscription.Expiration_Date,
                subscription.Active,
                subscription.Company_Id,
            };

            return await _db.Modify<dynamic>(Subscription_Procedure.Create, data);
        }

        public async Task<int> Update(Subscription subscription)
        {
            var data = new
            {
                subscription.Id,
                subscription.Start_Date,
                subscription.Expiration_Date,
                subscription.Active,
                subscription.Company_Id,
            };

            return await _db.Modify<dynamic>(Subscription_Procedure.Update, data);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _db.Modify<dynamic>(Subscription_Procedure.Delete, new { Id = id });
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }
    }
}
