CREATE TABLE [dbo].[Subscription]
(
    [Id]            		INT IDENTITY (1, 1) NOT NULL,
    [Start_Date]            DATE NOT NULL,
    [Expiration_Date]       DATE NULL,
    [Active]                BIT NOT NULL,
    [Company_Id]            INT NOT NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Company_Id]) REFERENCES [Company] ([Id])

)
