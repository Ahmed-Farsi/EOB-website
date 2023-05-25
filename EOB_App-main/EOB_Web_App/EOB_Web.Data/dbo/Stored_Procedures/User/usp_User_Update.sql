CREATE PROCEDURE [dbo].[usp_User_Update]
	@Id INT OUTPUT,
	@Email_Address NVARCHAR(128),
    @Role_Name NVARCHAR(128),
    @Verified BIT,
    @Admin_Recieve_Email BIT,
    @Company_Id INT
AS
BEGIN
	UPDATE [User] SET 
		[Email_Address] = @Email_Address,
		[Role_Name] = @Role_Name,
		[Verified] = @Verified,
		[Admin_Recieve_Email] = @Admin_Recieve_Email,
		[Company_Id] = @Company_Id
	OUTPUT INSERTED.[Id]
    WHERE 
		[Id] = @Id;
END
