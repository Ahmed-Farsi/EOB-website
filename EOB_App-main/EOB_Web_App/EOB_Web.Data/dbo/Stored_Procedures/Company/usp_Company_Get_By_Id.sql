CREATE PROCEDURE [dbo].[usp_Company_Get_By_Id]
    @Id INT
AS
BEGIN
    SELECT * FROM [Company] WHERE [Id] = @Id;
END
