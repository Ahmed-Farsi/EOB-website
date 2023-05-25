CREATE PROCEDURE [dbo].[usp_Group_Get_All]
AS
BEGIN
	SELECT 
		[Group].*, [Company].*
	FROM 
		[Group]
	LEFT JOIN
		[Company] ON [Group].[Company_Id] = [Company].[Id];
END
