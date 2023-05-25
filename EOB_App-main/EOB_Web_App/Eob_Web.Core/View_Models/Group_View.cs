using System;
using System.ComponentModel.DataAnnotations;

namespace Eob_Web.Core.View_Models
{
    public class Group_View
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(128, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department is Required")]
        [StringLength(128, ErrorMessage = "Department is too long")]
        public string Department { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Company is Required")]
        public int Company_Id { get; set; }
    }
}
