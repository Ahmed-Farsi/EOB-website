namespace Eob_Web.Core
{
    public enum Load_Status
    { 
        Loading,
        Empty,
        Success,
    }

    public enum Form_Status
    { 
        None,
        New,
        Edit,
        Disabled,
    }

    public enum Popup_Status
    { 
        None,
        Error,
        Delete,
        Data,

        Exists,
        Non_Existent,
        Foreign_Key_Error,
        Ip_Range_Error,
        Verified_New_User,
        Unverified_Admin,
        No_Company,
        New_User_Company,
        Bad_Subscription,
    }

    public static class User_Roles
    {
        public const string Admin = "Admin";
        public const string Company_Admin = "Company_Admin";
        public const string Employee = "Employee";
        public const string New_User = "New_User";
        public const string Eob = "Eob";

        //Role groups
        public const string All = "Admin,Company_Admin,Employee,New_User";
        public const string Company = "Company_Admin,Employee";
        public const string Admins = "Admin,Company_Admin";
        public const string New = "Company_Admin,Employee,New_User";
        public const string Humans = "Admin,Company_Admin,Employee"; // Everyone except EOB and New_User
    }
}
