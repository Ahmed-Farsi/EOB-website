using System.ComponentModel.DataAnnotations;

namespace Eob_Web.Core.View_Models
{
    public class ZT_Node_View
    {
        public string Node_Name { get; set; }
        public string Node_Description { get; set; }
        public string Network_Id { get; set; }
        public string Node_Id { get; set; }

        [RegularExpression(@"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.){3}(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$",
            ErrorMessage = "Please enter a valid ip address")]
        public string Node_Ip { get; set; }
        public bool Authorized { get; set; }
    }
}
