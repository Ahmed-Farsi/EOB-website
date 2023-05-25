CREATE PROCEDURE [dbo].[usp_Group_Get_All_By_Company_Id]
    @Company_Id INT
AS
BEGIN
	SELECT 
		[Group].*, [Company].*
	FROM 
		[Group]
	LEFT JOIN
		[Company] ON [Group].[Company_Id] = [Company].[Id]
	WHERE
		[Company].[Id] = @Company_Id
END
