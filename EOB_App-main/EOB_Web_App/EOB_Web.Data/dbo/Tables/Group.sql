CREATE TABLE [dbo].[Group] (
    [Id]                    INT IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR(128) NOT NULL,
    [Department]            NVARCHAR(128) NOT NULL,
    [Company_Id]            INT NOT NULL

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Company_Id]) REFERENCES [Company] ([Id])
);