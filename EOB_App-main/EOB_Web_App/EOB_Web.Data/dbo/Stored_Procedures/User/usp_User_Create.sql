CREATE PROCEDURE [dbo].[usp_User_Create]
	@Id INT OUTPUT,
	@Email_Address NVARCHAR(128),
	@Role_Name NVARCHAR(128),
	@Verified BIT,
	@Admin_Recieve_Email BIT,
	@Company_Id INT
AS
BEGIN
	INSERT INTO [User]
	(
		[Email_Address],
		[Role_Name],
		[Verified],
		[Admin_Recieve_Email],
		[Company_Id]
	)
	OUTPUT INSERTED.[Id]
	VALUES
	(
		@Email_Address,
		@Role_Name,
		@Verified,
		@Admin_Recieve_Email,
		@Company_Id
	);
END
