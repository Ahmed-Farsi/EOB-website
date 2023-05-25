CREATE PROCEDURE [dbo].[usp_Company_Update]
	@Id INT OUTPUT,
	@Name NVARCHAR(128),
    @Address NVARCHAR(128),
    @Email_Address NVARCHAR(128),
    @Phone_Number NVARCHAR(128),
    @Invite_Code UNIQUEIDENTIFIER
AS
BEGIN
	UPDATE [Company] SET 
		[Name] = @Name,
		[Address] = @Address,
		[Email_Address] = @Email_Address,
		[Phone_Number] = @Phone_Number,
		[Invite_Code] = @Invite_Code
	OUTPUT INSERTED.[Id]
    WHERE 
		[Id] = @Id;
END
