CREATE PROCEDURE [dbo].[usp_Company_Create]
	@Id INT OUTPUT,
	@Name VARCHAR(128),
	@Address VARCHAR(128),
	@Email_Address VARCHAR(128),
	@Phone_Number VARCHAR(128),
	@Invite_Code UNIQUEIDENTIFIER
AS
BEGIN
	INSERT INTO [Company]
	(
		[Name],
		[Address],
		[Email_Address],
		[Phone_Number],
		[Invite_Code]
	)
	OUTPUT INSERTED.[Id]
	VALUES
	(
		@Name,
		@Address,
		@Email_Address,
		@Phone_Number,
		@Invite_Code
	);
END
