CREATE PROCEDURE [dbo].[usp_Eob_Create]
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
	INSERT INTO [Eob]
    (
		[Serial_Number],
        [Network_Id],
        [Node_Id],
        [Node_Name],
        [Company_Id], 
        [Group_Id],
        [Subscription_Id]
    )
	OUTPUT INSERTED.[Id]
	VALUES
    (
		@Serial_Number,
        @Network_Id,
        @Node_Id,
        @Node_Name,
        @Company_Id,
        @Group_Id,
        @Subscription_Id
    );
END
