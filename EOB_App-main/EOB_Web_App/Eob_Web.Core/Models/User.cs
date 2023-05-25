using System.Collections.Generic;

namespace Eob_Web.Core.Models
{
    public class User
    {
        public User()
        {
            Groups = new List<Group>();
        }

        public int Id { get; set; }
        public string Email_Address { get; set; }
        public string Role_Name { get; set; }
        public bool Verified { get; set; }

        /// <summary>
        /// Admin only: Get notified by email when new_user registers a company.
        /// </summary>
        public bool Admin_Recieve_Email { get; set; }

        public int? Company_Id { get; set; }
        public Company? Company { get; set; }

        public List<Group> Groups { get; set; }
    }
}
