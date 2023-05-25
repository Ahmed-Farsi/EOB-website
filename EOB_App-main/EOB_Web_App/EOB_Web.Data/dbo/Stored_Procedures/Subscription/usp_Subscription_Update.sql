CREATE PROCEDURE [dbo].[usp_Subscription_Update]
	@Id INT OUTPUT,
	@Start_Date DATE,
    @Expiration_Date DATE,
    @Active BIT,
    @Company_Id INT
AS
BEGIN
	UPDATE [Subscription] SET 
		[Start_Date] = @Start_Date,
		[Expiration_Date] = @Expiration_Date,
		[Active] = @Active,
		[Company_Id] = @Company_Id
	OUTPUT INSERTED.[Id]
    WHERE 
		[Id] = @Id;
END
