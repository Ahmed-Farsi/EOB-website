CREATE PROCEDURE [dbo].[usp_Group_Get_By_Company_Id]
    @Id INT,
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
		[Group].[Id] = @Id
	AND 
		[Company].[Id] = @Company_Id;
END
