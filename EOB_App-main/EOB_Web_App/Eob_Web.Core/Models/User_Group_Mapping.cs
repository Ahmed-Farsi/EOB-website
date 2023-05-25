namespace Eob_Web.Core.Models
{
    /// <summary>
    /// Many to many mapping of users and groups
    /// </summary>
    public class User_Group_Mapping
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Group_Id { get; set; }
    }
}
