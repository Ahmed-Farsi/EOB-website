CREATE PROCEDURE [dbo].[usp_Subscription_Get_All_By_Company_Id]
    @Company_Id INT
AS
BEGIN
	SELECT 
		[Subscription].*, [Company].*
	FROM 
		[Subscription]
	LEFT JOIN
		[Company] ON [Subscription].[Company_Id] = [Company].[Id]
	WHERE
		[Company].[Id] = @Company_Id
END
