CREATE PROCEDURE [dbo].[usp_User_Get_All_User_Group]
	@Id INT OUTPUT,
	@User_Id INT,
	@Group_Id INT
AS
BEGIN
	SELECT * FROM [User_Group]
	WHERE
		[User_Id] = @User_Id
	AND
		[Group_Id] = @Group_Id;
END
