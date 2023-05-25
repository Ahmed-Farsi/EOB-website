using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Models
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

        public int? Company_Id { get; set; }
        public Company? Company { get; set; }

        public List<Group> Groups { get; set; }
    }
}
