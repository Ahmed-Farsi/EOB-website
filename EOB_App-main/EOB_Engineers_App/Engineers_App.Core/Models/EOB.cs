using Engineers_App.Core.Models.ZeroTier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineers_App.Core.Models
{
    public class Eob
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public string Network_Id { get; set; }
        public string Node_Id { get; set; }
        public string Node_Name { get; set; }

        public int Company_Id { get; set; }
        public Company Company { get; set; }

        public int? Group_Id { get; set; }
        public Group? Group { get; set; }

        public int? Subscription_Id { get; set; }
        public Subscription? Subscription { get; set; }
        public ZT_Node? Node { get; set; }
    }
}
