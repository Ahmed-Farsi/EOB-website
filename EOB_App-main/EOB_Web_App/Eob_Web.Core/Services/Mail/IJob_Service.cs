using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eob_Web.Core.Services
{
    public interface IJob_Service
    {
        Task Email_Company_Admin_Joined(string company_Admin_Email, List<string> admin_Emails, string message);
        Task Email_Contact(List<string> admin_Emails, string message);
        Task Email_Employee_Joined(string company_Admin_Email, string message);
        Task Email_Subscription_Expiration_Remider();
        Task Email_Subscription_Expired();
    }
}