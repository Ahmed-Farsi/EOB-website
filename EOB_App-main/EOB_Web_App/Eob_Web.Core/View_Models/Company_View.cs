using System;
using System.ComponentModel.DataAnnotations;

namespace Eob_Web.Core.View_Models
{
    public class Company_View
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(128, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        [StringLength(128, ErrorMessage = "Address is too long")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(128, ErrorMessage = "Email address is too long")]
        [EmailAddress]
        public string Email_Address { get; set; }

        [Required(ErrorMessage = "Phone number is Required")]
        [StringLength(128, ErrorMessage = "Phone number is too long")]
        [Phone]
        public string Phone_Number { get; set; }

        public Guid Invite_Code { get; set; }
        public string Invite_Code_String { get; set; }
    }
}
