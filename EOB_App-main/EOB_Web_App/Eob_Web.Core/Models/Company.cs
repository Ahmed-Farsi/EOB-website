using System;

namespace Eob_Web.Core.Models
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
