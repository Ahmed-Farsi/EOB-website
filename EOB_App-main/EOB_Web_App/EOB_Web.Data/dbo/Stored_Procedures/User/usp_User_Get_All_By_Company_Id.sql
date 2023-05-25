CREATE PROCEDURE [dbo].[usp_User_Get_All_By_Company_Id]
    @Company_Id INT
AS
BEGIN
	SELECT 
		[User].*, [Company].*, [Group].*
	FROM 
		[User]
	LEFT JOIN
		[Company] ON [User].[Company_Id] = [Company].[Id]
	LEFT JOIN
		[User_Group] ON [User_Group].[User_Id] = [User].[Id]
	LEFT JOIN
		[Group] ON [Group].[Id] = [User_Group].[Group_Id]
	WHERE
		[Company].[Id] = @Company_Id
END
