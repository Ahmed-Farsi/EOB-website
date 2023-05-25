using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email_Address { get; set; }
        public string Phone_Number { get; set; }
        public Guid Invite_Code { get; set; }
    }
}
