using System;

namespace Eob_Web.Core.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public bool Active { get; set; }

        public int Company_Id { get; set; }
        public Company Company { get; set; }
    }
}
