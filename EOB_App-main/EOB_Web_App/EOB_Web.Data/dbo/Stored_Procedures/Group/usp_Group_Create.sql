CREATE PROCEDURE [dbo].[usp_Group_Create]
	@Id INT OUTPUT,
	@Name NVARCHAR(128),
	@Department NVARCHAR(128),
	@Company_Id INT
AS
BEGIN
	INSERT INTO [Group]
	(
		[Name],
		[Department],
		[Company_Id]
	)
	OUTPUT INSERTED.[Id]
	VALUES
	(
		@Name,
		@Department,
		@Company_Id
	);
END
