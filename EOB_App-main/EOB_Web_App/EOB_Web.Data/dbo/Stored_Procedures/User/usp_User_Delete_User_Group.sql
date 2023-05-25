CREATE PROCEDURE [dbo].[usp_User_Delete_User_Group]
	@Id INT OUTPUT,
	@User_Id INT,
	@Group_Id INT 
AS
BEGIN
	DELETE FROM [User_Group]
	OUTPUT DELETED.[Id]
	WHERE 
		[User_Id] = @User_Id
	AND
		[Group_Id] = @Group_Id;
END
