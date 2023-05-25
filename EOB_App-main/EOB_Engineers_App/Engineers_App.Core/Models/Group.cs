using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public int Company_Id { get; set; }
        public Company Company { get; set; }
    }
}
