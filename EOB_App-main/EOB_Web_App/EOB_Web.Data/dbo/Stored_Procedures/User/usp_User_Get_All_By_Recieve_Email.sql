CREATE PROCEDURE [dbo].[usp_User_Get_All_By_Recieve_Email]
AS
BEGIN
	SELECT 
		[User].Email_Address
	FROM 
		[User]
	WHERE
		[User].[Admin_Recieve_Email] = 1;
END
