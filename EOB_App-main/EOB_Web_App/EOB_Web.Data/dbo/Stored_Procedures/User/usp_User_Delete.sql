﻿CREATE PROCEDURE [dbo].[usp_User_Delete]
	@Id INT OUTPUT
AS
BEGIN
	DELETE FROM [User]
	OUTPUT DELETED.[Id]
	WHERE [Id] = @Id;
END