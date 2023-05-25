CREATE PROCEDURE [dbo].[usp_Subscription_Create]
	@Id INT OUTPUT,
	@Start_Date DATE,
	@Expiration_Date DATE,
	@Active BIT,
	@Company_Id INT
AS
BEGIN
	INSERT INTO [Subscription]
	(
		[Start_Date],
		[Expiration_Date],
		[Active],
		[Company_Id]
	)
	OUTPUT INSERTED.[Id]
	VALUES
	(
		@Start_Date,
		@Expiration_Date,
		@Active,
		@Company_Id
	);
END
