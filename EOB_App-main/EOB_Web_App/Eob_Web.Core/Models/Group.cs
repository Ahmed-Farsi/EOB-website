namespace Eob_Web.Core.Models
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
