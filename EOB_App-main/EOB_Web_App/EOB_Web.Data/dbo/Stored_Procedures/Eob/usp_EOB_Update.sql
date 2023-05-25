CREATE PROCEDURE [dbo].[usp_Eob_Update]
	@Id INT OUTPUT,
	@Serial_Number NVARCHAR(128),
    @Network_Id NVARCHAR(128),
    @Node_Id NVARCHAR(128),
    @Node_Name NVARCHAR(128),
    @Company_Id INT,
    @Group_Id INT,
    @Subscription_Id INT
AS
BEGIN
	UPDATE [Eob] SET 
		[Serial_Number] = @Serial_Number,
		[Network_Id] = @Network_Id,
		[Node_Id] = @Node_Id,
		[Node_Name] = @Node_Name,
		[Company_Id] = @Company_Id,
		[Group_Id] = @Group_Id,
		[Subscription_Id] = @Subscription_Id
	OUTPUT INSERTED.[Id]
    WHERE 
		[Id] = @Id;
END
