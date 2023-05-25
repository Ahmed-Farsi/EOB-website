namespace Eob_Web.Core
{
    public static class Eob_Procedure
    {
        public const string Get_All = "dbo.usp_Eob_Get_All";
        public const string Get_By_Id = "dbo.usp_Eob_Get_By_Id";
        public const string Get_By_Company_Id = "dbo.usp_Eob_Get_By_Company_Id";
        public const string Get_All_By_Company_Id = "dbo.usp_Eob_Get_All_By_Company_Id";
        public const string Get_By_Node_Id = "dbo.usp_Eob_Get_By_Node_Id";
        public const string Get_By_Network_Id = "dbo.usp_Eob_Get_By_Network_Id";
        public const string Create = "dbo.usp_Eob_Create";
        public const string Update = "dbo.usp_Eob_Update";
        public const string Delete = "dbo.usp_Eob_Delete";
    }

    public static class Company_Procedure
    {
        public const string Get_All = "dbo.usp_Company_Get_All";
        public const string Get_By_Id = "dbo.usp_Company_Get_By_Id";
        public const string Get_By_Company_Id = "dbo.usp_Company_Get_By_Company_Id";
        public const string Get_By_Invite_Code = "dbo.usp_Company_Get_By_Invite_Code";
        public const string Create = "dbo.usp_Company_Create";
        public const string Update = "dbo.usp_Company_Update";
        public const string Delete = "dbo.usp_Company_Delete";
    }

    public static class Group_Procedure
    {
        public const string Get_All = "dbo.usp_Group_Get_All";
        public const string Get_By_Id = "dbo.usp_Group_Get_By_Id";
        public const string Get_By_Company_Id = "dbo.usp_Group_Get_By_Company_Id";
        public const string Get_All_By_Company_Id = "dbo.usp_Group_Get_All_By_Company_Id";
        public const string Create = "dbo.usp_Group_Create";
        public const string Update = "dbo.usp_Group_Update";
        public const string Delete = "dbo.usp_Group_Delete";
    }

    public static class User_Procedure
    {
        public const string Get_All = "dbo.usp_User_Get_All";
        public const string Get_All_User_Group = "dbo.usp_User_Get_All_User_Group";
        public const string Get_By_Id = "dbo.usp_User_Get_By_Id";
        public const string Get_By_Company_Id = "dbo.usp_User_Get_By_Company_Id";
        public const string Get_All_By_Company_Id = "dbo.usp_User_Get_All_By_Company_Id";
        public const string Get_All_By_Recieve_Email = "dbo.usp_User_Get_All_By_Recieve_Email";
        public const string Create = "dbo.usp_User_Create";
        public const string Create_User_Group = "dbo.usp_User_Create_User_Group";
        public const string Update = "dbo.usp_User_Update";
        public const string Delete = "dbo.usp_User_Delete";
        public const string Delete_User_Group = "dbo.usp_User_Delete_User_Group";
        public const string Delete_All_User_Group = "dbo.usp_User_Delete_All_User_Group";
    }

    public static class Subscription_Procedure
    {
        public const string Get_All = "dbo.usp_Subscription_Get_All";
        public const string Get_By_Id = "dbo.usp_Subscription_Get_By_Id";
        public const string Get_By_Company_Id = "dbo.usp_Subscription_Get_By_Company_Id";
        public const string Get_All_By_Company_Id = "dbo.usp_Subscription_Get_All_By_Company_Id";
        public const string Create = "dbo.usp_Subscription_Create";
        public const string Update = "dbo.usp_Subscription_Update";
        public const string Delete = "dbo.usp_Subscription_Delete";
    }
}
