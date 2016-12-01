CREATE TABLE [dbo].[Reviews] (
    [id]        INT           IDENTITY (1, 1) NOT NULL,
    [Name]      VARCHAR (255) DEFAULT ('Anonymous') NULL,
    [Review]    VARCHAR (255) NULL,
    [Rating]    INT           NOT NULL,
    [ProductID] INT           NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);