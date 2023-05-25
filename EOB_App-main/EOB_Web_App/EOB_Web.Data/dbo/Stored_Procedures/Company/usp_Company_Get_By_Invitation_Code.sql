CREATE PROCEDURE [dbo].[usp_Company_Get_By_Invite_Code]
    @Invite_Code UNIQUEIDENTIFIER
AS
BEGIN
    SELECT * FROM [Company] WHERE [Invite_Code] = @Invite_Code;
END
