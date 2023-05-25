using Eob_Web.Core.Models;
using Eob_Web.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eob_Web.Core.Mappers
{
    public class Company_Mapper
    {
        public Company View_To_Model(Company_View company_View)
        {
            var company = new Company
            {
                Id = company_View.Id,
                Name = company_View.Name,
                Address = company_View.Address,
                Email_Address = company_View.Email_Address,
                Phone_Number = company_View.Phone_Number,
                Invite_Code = company_View.Invite_Code,
            };

            return company;
        }

        public void Model_To_View(Company company, Company_View company_View)
        {
            company_View.Id = company.Id;
            company_View.Name = company.Name;
            company_View.Address = company.Address;
            company_View.Email_Address = company.Email_Address;
            company_View.Phone_Number = company.Phone_Number;
            company_View.Invite_Code = company.Invite_Code;
        }
    }
}
