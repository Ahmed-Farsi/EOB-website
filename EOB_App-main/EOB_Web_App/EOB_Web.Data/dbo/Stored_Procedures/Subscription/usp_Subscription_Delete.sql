CREATE PROCEDURE [dbo].[usp_Subscription_Delete]
	@Id INT OUTPUT
AS
BEGIN
	DELETE FROM [Subscription]
	OUTPUT DELETED.[Id]
	WHERE [Id] = @Id;
END
