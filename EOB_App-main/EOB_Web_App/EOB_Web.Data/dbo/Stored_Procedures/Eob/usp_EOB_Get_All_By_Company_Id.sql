CREATE PROCEDURE [dbo].[usp_Eob_Get_All_By_Company_Id]
	@Company_Id INT
AS
BEGIN
	SELECT 
		[Eob].*, [Company].*, [Group].*, [Subscription].*
	FROM 
		[Eob]
	LEFT JOIN
		[Company] ON [Eob].[Company_Id] = [Company].[Id]
	LEFT JOIN
		[Group] ON [Eob].[Group_Id] = [Group].[Id]
	LEFT JOIN
		[Subscription] ON [Eob].[Subscription_Id] = [Subscription].[Id]
	WHERE
		[Company].[Id] = @Company_Id;
END
