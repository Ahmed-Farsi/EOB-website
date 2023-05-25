CREATE TABLE [dbo].[User] (
    [Id]                    INT IDENTITY (1, 1) NOT NULL,
    [Email_Address]         NVARCHAR(128) NOT NULL,
    [Role_Name]             NVARCHAR(128) NOT NULL,
    [Verified]              BIT NOT NULL,
    [Admin_Recieve_Email]   BIT NOT NULL,
    [Company_Id]            INT NULL

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Company_Id]) REFERENCES [Company] ([Id])
);