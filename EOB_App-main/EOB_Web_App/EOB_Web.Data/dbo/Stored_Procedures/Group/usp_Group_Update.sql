CREATE PROCEDURE [dbo].[usp_Group_Update]
	@Id INT OUTPUT,
	@Name NVARCHAR(128),
    @Department NVARCHAR(128),
    @Company_Id INT
AS
BEGIN
	UPDATE [Group] SET 
		[Name] = @Name,
		[Department] = @Department,
		[Company_Id] = @Company_Id
	OUTPUT INSERTED.[Id]
    WHERE 
		[Id] = @Id;
END
