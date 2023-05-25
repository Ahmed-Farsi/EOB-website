using System.ComponentModel.DataAnnotations;

namespace Eob_Web.Core.View_Models
{
    public class Contact_View
    {
        [StringLength(128, ErrorMessage = "Name is too long")]
        public string Customer_Name { get; set; }

        [EmailAddress]
        [StringLength(128, ErrorMessage = "Email address is too long")]
        public string Email_Address { get; set; }
        [StringLength(128, ErrorMessage = "Subject is too long")]
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
