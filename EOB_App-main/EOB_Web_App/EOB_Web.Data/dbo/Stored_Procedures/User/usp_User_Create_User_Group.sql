CREATE PROCEDURE [dbo].[usp_User_Create_User_Group]
	@Id INT OUTPUT,
	@User_Id INT,
	@Group_Id INT
AS
BEGIN
	INSERT INTO [User_Group]
	(
		[User_Id],
		[Group_Id]
	)
	OUTPUT INSERTED.[Id]
	VALUES
	(
		@User_Id,
		@Group_Id
	);
END
