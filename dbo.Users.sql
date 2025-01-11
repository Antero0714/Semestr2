CREATE TABLE [dbo].[Users] (
    [Id]       INT           NOT NULL,
    [Login]    NVARCHAR (50) NULL,
    [Password] NVARCHAR (50) NULL,

    PRIMARY KEY CLUSTERED ([Id] ASC)
);

