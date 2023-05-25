using System.ComponentModel.DataAnnotations;

namespace Eob_Web.Core.View_Models
{
    public class User_View
    {
        public int Id { get; set; }
        public string Email_Address { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Role_Name { get; set; }

        public bool Verified { get; set; }
        public bool Admin_Recieve_Email { get; set; }

        public int Company_Id { get; set; }
    }
}
