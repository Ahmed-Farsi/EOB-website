CREATE PROCEDURE [dbo].[usp_User_Delete_All_User_Group]
	@Id INT OUTPUT,
	@User_Id INT
AS
BEGIN
	DELETE FROM [User_Group]
	OUTPUT DELETED.[Id]
	WHERE 
		[User_Id] = @User_Id;
END
