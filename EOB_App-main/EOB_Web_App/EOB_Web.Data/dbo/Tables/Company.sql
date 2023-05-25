CREATE TABLE [dbo].[Company] (
    [Id]            INT IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR(128) NOT NULL, 
    [Address]       NVARCHAR(128) NOT NULL, 
    [Email_Address] NVARCHAR(128) NOT NULL, 
    [Phone_Number]  NVARCHAR(128) NOT NULL,
    [Invite_Code]   UNIQUEIDENTIFIER NOT NULL

    PRIMARY KEY CLUSTERED ([Id] ASC)
)
