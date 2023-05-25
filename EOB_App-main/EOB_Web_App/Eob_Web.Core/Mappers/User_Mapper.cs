using Eob_Web.Core.Models;
using Eob_Web.Core.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eob_Web.Core.Mappers
{
    public class User_Mapper
    {
        public User View_To_Model(User_View user_View)
        {
            var user = new User
            {
                Id = user_View.Id,
                Email_Address = user_View.Email_Address,
                Role_Name = user_View.Role_Name,
                Verified = user_View.Verified,
                Admin_Recieve_Email = user_View.Admin_Recieve_Email,
                Company_Id = user_View.Company_Id
            };

            if (user.Company_Id == 0)
            { 
                user.Company_Id = null;
            }

            return user;
        }

        public void Model_To_View(User user, User_View user_View)
        {
            user_View.Id = user.Id;
            user_View.Email_Address = user.Email_Address;
            user_View.Role_Name = user.Role_Name;
            user_View.Admin_Recieve_Email = user.Admin_Recieve_Email;
            user_View.Verified = user.Verified;

            if (user.Company_Id.HasValue)
            {
                user_View.Company_Id = user.Company_Id.Value;
            }
            else
            {
                user_View.Company_Id = 0;
            }
        }
    }
}
