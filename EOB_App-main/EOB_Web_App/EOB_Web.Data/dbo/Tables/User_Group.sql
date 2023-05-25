CREATE TABLE [dbo].[User_Group] (
    [Id]                    INT IDENTITY (1, 1) NOT NULL,
    [User_Id]               INT NOT NULL,
    [Group_Id]              INT NOT NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([User_Id]) REFERENCES [User] ([Id]),
    FOREIGN KEY ([Group_Id]) REFERENCES [Group] ([Id])
);