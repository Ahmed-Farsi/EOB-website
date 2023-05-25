using System;
using System.ComponentModel.DataAnnotations;

namespace Eob_Web.Core.View_Models
{
    public class Eob_View
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Serial number is required")]
        [StringLength(128, ErrorMessage = "Serial number is too long")]
        public string Serial_Number { get; set; }

        /* -------------------- ZeroTier -------------------- */
        [Required(ErrorMessage = "Node ID is required")]
        [StringLength(10, ErrorMessage = "Node ID must be 10 characters long")]
        [MinLength(10, ErrorMessage = "Node ID must be 10 characters long")]
        public string Node_Id { get; set; }
        public string Network_Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(128, ErrorMessage = "Name is too long")]
        public string Node_Name { get; set; }
        /* -------------------------------------------------- */

        public bool Subscription_Active { get; set; }
        public DateTime? Subscription_Expiration { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Company is Required")]
        public int Company_Id { get; set; }
        public int Group_Id { get; set; }
        public int Subscription_Id { get; set; }
    }
}
