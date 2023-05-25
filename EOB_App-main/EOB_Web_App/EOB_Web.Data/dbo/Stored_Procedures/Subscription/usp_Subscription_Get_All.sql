CREATE PROCEDURE [dbo].[usp_Subscription_Get_All]
AS
BEGIN
	SELECT 
		[Subscription].*, [Company].*
	FROM 
		[Subscription]
	LEFT JOIN
		[Company] ON [Subscription].[Company_Id] = [Company].[Id];
END
