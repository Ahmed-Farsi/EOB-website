using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eob_Web.Core.Models;

namespace Eob_Web.Core.Services
{
    public class User_Service : IUser_Service
    {
        private readonly IRepository _db;

        public User_Service(IRepository db)
        {
            _db = db;
        }

        // the mapping that we need to pass to dapper in order to join objects
        private static User Mapping(User user, Company company, Group group)
        {
            user.Company = company;
            if (group != null)
            { 
                user.Groups.Add(group);
            }

            return user;
        }

        public async Task<List<User>> Get_All()
        {
            var result = await _db.Load_Multi<User, Company, Group, User, dynamic>(
                User_Procedure.Get_All,
                new { },
                Mapping);

            var grouping = result
                .GroupBy(user => user.Id)
                .Select(group =>
                {
                    var grouped_User = group.FirstOrDefault();

                    if (!grouped_User.Groups.Any())
                    { 
                        return grouped_User;
                    }

                    grouped_User.Groups = group.Select(user => user.Groups.SingleOrDefault()).ToList();
                    return grouped_User;
                });

            return grouping.ToList();
        }

        public async Task<List<User_Group_Mapping>> Get_All_User_Group(User_Group_Mapping user_Group)
        {
            var data = new
            {
                Id = user_Group.Id,
                User_Id = user_Group.User_Id,
                Group_Id = user_Group.Group_Id
            };

            var result = await _db.Load<User_Group_Mapping, dynamic>(
                User_Procedure.Get_All_User_Group,
                data);

            return result.ToList();
        }

        public async Task<User> Get_By_Id(int id)
        {
            var result = await _db.Load_Multi<User, Company, Group, User, dynamic>(
                User_Procedure.Get_By_Id,
                new { Id = id },
                Mapping);

            var grouping = result
                .GroupBy(user => user.Id)
                .Select(group =>
                {
                    var grouped_User = group.FirstOrDefault();

                    if (!grouped_User.Groups.Any())
                    { 
                        return grouped_User;
                    }

                    grouped_User.Groups = group.Select(user => user.Groups.SingleOrDefault()).ToList();
                    return grouped_User;
                });

            return grouping.FirstOrDefault();
        }

        public async Task<List<User>> Get_All_By_Company_Id(int company_Id)
        {
            var result = await _db.Load_Multi<User, Company, Group, User, dynamic>(
                User_Procedure.Get_All_By_Company_Id,
                new { Company_Id = company_Id },
                Mapping);

            var grouping = result
                .GroupBy(user => user.Id)
                .Select(group =>
                {
                    var grouped_User = group.FirstOrDefault();

                    if (!grouped_User.Groups.Any())
                    { 
                        return grouped_User;
                    }

                    grouped_User.Groups = group.Select(user => user.Groups.SingleOrDefault()).ToList();
                    return grouped_User;
                });

            return grouping.ToList();
        }

        public async Task<User> Get_By_Company_Id(int id, int company_Id)
        {
            var result = await _db.Load_Multi<User, Company, Group, User, dynamic>(
                User_Procedure.Get_By_Company_Id,
                new { Id = id, Company_Id = company_Id },
                Mapping);

            var grouping = result
                .GroupBy(user => user.Id)
                .Select(group =>
                {
                    var grouped_User = group.FirstOrDefault();

                    if (!grouped_User.Groups.Any())
                    { 
                        return grouped_User;
                    }

                    grouped_User.Groups = group.Select(user => user.Groups.SingleOrDefault()).ToList();
                    return grouped_User;
                });

            return grouping.FirstOrDefault();
        }

        public async Task<List<string>> Get_All_By_Recieve_Email()
        {
            var result = await _db.Load<string, dynamic>(
                User_Procedure.Get_All_By_Recieve_Email,
                new { });

            return result.ToList();
        }

        public async Task<List<User>> Search(string term)
        {
            var result = await _db.Load_Multi<User, Company, Group, User, dynamic>(
                User_Procedure.Get_All,
                new { },
                Mapping);

            var grouping = result
                .GroupBy(user => user.Id)
                .Select(group =>
                {
                    var grouped_User = group.FirstOrDefault();

                    if (!grouped_User.Groups.Any())
                    { 
                        return grouped_User;
                    }

                    grouped_User.Groups = group.Select(user => user.Groups.SingleOrDefault()).ToList();
                    return grouped_User;
                });

            if (string.IsNullOrEmpty(term))
                return grouping.ToList();

            return grouping.Where(x => x.Email_Address.ToLower().Contains(term.ToLower()) ||
                                       x.Role_Name.ToLower().Contains(term.ToLower()) ||
                                       x.Company_Id.HasValue && x.Company.Name.ToLower().Contains(term.ToLower()))
                .ToList();
        }

        public async Task<int> Create(User user)
        {
            var data = new
            {
                user.Id,
                user.Email_Address,
                user.Role_Name,
                user.Verified,
                user.Admin_Recieve_Email,
                user.Company_Id,
            };

            return await _db.Modify<dynamic>(User_Procedure.Create, data);
        }

        public async Task<int> Update(User user)
        {
            var data = new
            {
                user.Id,
                user.Email_Address,
                user.Role_Name,
                user.Verified,
                user.Admin_Recieve_Email,
                user.Company_Id,
            };

            return await _db.Modify<dynamic>(User_Procedure.Update, data);
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                await _db.Modify<dynamic>(User_Procedure.Delete, new { Id = id });
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }

        /* ---------------------------------------------
         *
         *  User_Group section (many to many)
         * 
         *  --------------------------------------------- */
        public async Task<int> Create_User_Group(User_Group_Mapping user_Group)
        {
            var data = new
            {
                user_Group.Id,
                user_Group.User_Id,
                user_Group.Group_Id
            };

            return await _db.Modify<dynamic>(User_Procedure.Create_User_Group, data);
        }

        public async Task<bool> Delete_User_Group(User_Group_Mapping user_Group)
        {
            var data = new
            {
                user_Group.Id,
                user_Group.User_Id,
                user_Group.Group_Id
            };

            try
            {
                await _db.Modify<dynamic>(User_Procedure.Delete_User_Group, data);
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }

        public async Task<bool> Delete_All_User_Group(User user)
        {
            var data = new
            {
                Id = user.Id,
                User_Id = user.Id,
            };

            try
            {
                await _db.Modify<dynamic>(User_Procedure.Delete_All_User_Group, data);
                return true;
            }
            catch (SqlException) // Foreign key check
            {
                return false;
            }
        }
    }
}
