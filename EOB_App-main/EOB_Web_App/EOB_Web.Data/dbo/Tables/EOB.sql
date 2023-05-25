CREATE TABLE [dbo].[Eob] (
    [Id]            		INT IDENTITY (1, 1) NOT NULL,
    [Serial_Number] 		NVARCHAR(128) NOT NULL,
    [Network_Id]        	NVARCHAR(128) NOT NULL,
    [Node_Id]           	NVARCHAR(128) NOT NULL,
    [Node_Name]         	NVARCHAR(128) NOT NULL,
    [Company_Id]        	INT NOT NULL,
    [Group_Id]          	INT NULL,
    [Subscription_Id]      	INT NULL

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([Company_Id]) REFERENCES [Company] ([Id]),
    FOREIGN KEY ([Group_Id]) REFERENCES [Group] ([Id]),
    FOREIGN KEY ([Subscription_Id]) REFERENCES [Subscription] ([Id])
);