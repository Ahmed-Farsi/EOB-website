CREATE PROCEDURE [dbo].[usp_Subscription_Get_By_Id]
    @Id INT
AS
BEGIN
	SELECT 
		[Subscription].*, [Company].*
	FROM 
		[Subscription]
	LEFT JOIN
		[Company] ON [Subscription].[Company_Id] = [Company].[Id]
	WHERE 
		[Subscription].[Id]= @Id;
END
