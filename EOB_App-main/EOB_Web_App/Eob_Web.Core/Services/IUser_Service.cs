using Eob_Web.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IUser_Service
    {
        Task<int> Create(User user);
        Task<int> Create_User_Group(User_Group_Mapping user_Group);
        Task<bool> Delete(int id);
        Task<bool> Delete_All_User_Group(User user);
        Task<bool> Delete_User_Group(User_Group_Mapping user_Group);
        Task<List<User>> Get_All();
        Task<List<User>> Get_All_By_Company_Id(int company_Id);
        Task<List<string>> Get_All_By_Recieve_Email();
        Task<List<User_Group_Mapping>> Get_All_User_Group(User_Group_Mapping user_Group);
        Task<User> Get_By_Company_Id(int id, int company_Id);
        Task<User> Get_By_Id(int id);
        Task<List<User>> Search(string term);
        Task<int> Update(User user);
    }
}