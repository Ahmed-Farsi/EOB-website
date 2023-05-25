using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eob_Web.Core.View_Models
{
    public class ZT_Network_View
    {
        [Required(ErrorMessage = "Subnet is required")]
        [RegularExpression(@"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.){3}(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\/\d{1,2}$",
            ErrorMessage = "Please enter a valid ip address")]
        public string Subnet { get; set; }
    }
}
