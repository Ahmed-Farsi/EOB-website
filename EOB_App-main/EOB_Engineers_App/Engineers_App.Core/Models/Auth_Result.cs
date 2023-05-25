using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Models
{
    public class Auth_Result
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
