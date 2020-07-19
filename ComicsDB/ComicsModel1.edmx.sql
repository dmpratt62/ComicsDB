
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/29/2019 08:03:21
-- Generated from EDMX file: C:\Users\davea\source\repos\dmpratt62\ComicsDB\ComicsDB\ComicsModel1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [J:\USERS\DAVEANDDEE\DOCUMENTS\COMICS\COMICS.MDF];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ComicAuthor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Authors] DROP CONSTRAINT [FK_ComicAuthor];
GO
IF OBJECT_ID(N'[dbo].[FK_ComicValuation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Valuations] DROP CONSTRAINT [FK_ComicValuation];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Comics]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comics];
GO
IF OBJECT_ID(N'[dbo].[Valuations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Valuations];
GO
IF OBJECT_ID(N'[dbo].[Authors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Authors];
GO
IF OBJECT_ID(N'[dbo].[Publishers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Publishers];
GO
IF OBJECT_ID(N'[dbo].[StorageBoxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StorageBoxes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Comics'
CREATE TABLE [dbo].[Comics] (
    [ComicsId] int IDENTITY(1,1) NOT NULL,
    [PublisherId] int  NOT NULL,
    [MainCharacter] nvarchar(max)  NOT NULL,
    [PublishDate] datetime  NOT NULL,
    [Illustrator] nvarchar(max)  NOT NULL,
    [IssueNumber] int  NOT NULL,
    [PhotoFileName] nvarchar(max)  NOT NULL,
    [SeriesTitle] nvarchar(max)  NOT NULL,
    [SeriesNumber] int  NOT NULL,
    [SecondaryCharacter] nvarchar(max)  NOT NULL,
    [Commemoration] nvarchar(max)  NOT NULL,
    [OriginalValue] decimal(18,0)  NOT NULL,
    [Condition] nvarchar(max)  NOT NULL,
    [StorageBoxId] int  NOT NULL,
    [Comments] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'Valuations'
CREATE TABLE [dbo].[Valuations] (
    [ValuationId] int IDENTITY(1,1) NOT NULL,
    [Value] decimal(18,0)  NOT NULL,
    [Condition] nvarchar(max)  NOT NULL,
    [Source] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [ComicComicsId] int  NOT NULL
);
GO

-- Creating table 'Authors'
CREATE TABLE [dbo].[Authors] (
    [AuthorId] int IDENTITY(1,1) NOT NULL,
    [FirstName] nvarchar(max)  NOT NULL,
    [LastName] nvarchar(max)  NOT NULL,
    [ComicComicsId] int  NOT NULL
);
GO

-- Creating table 'Publishers'
CREATE TABLE [dbo].[Publishers] (
    [PublisherId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Url] nvarchar(max)  NULL
);
GO

-- Creating table 'StorageBoxes'
CREATE TABLE [dbo].[StorageBoxes] (
    [StorageBoxId] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL,
    [Address] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ComicsId] in table 'Comics'
ALTER TABLE [dbo].[Comics]
ADD CONSTRAINT [PK_Comics]
    PRIMARY KEY CLUSTERED ([ComicsId] ASC);
GO

-- Creating primary key on [ValuationId] in table 'Valuations'
ALTER TABLE [dbo].[Valuations]
ADD CONSTRAINT [PK_Valuations]
    PRIMARY KEY CLUSTERED ([ValuationId] ASC);
GO

-- Creating primary key on [AuthorId] in table 'Authors'
ALTER TABLE [dbo].[Authors]
ADD CONSTRAINT [PK_Authors]
    PRIMARY KEY CLUSTERED ([AuthorId] ASC);
GO

-- Creating primary key on [PublisherId] in table 'Publishers'
ALTER TABLE [dbo].[Publishers]
ADD CONSTRAINT [PK_Publishers]
    PRIMARY KEY CLUSTERED ([PublisherId] ASC);
GO

-- Creating primary key on [StorageBoxId] in table 'StorageBoxes'
ALTER TABLE [dbo].[StorageBoxes]
ADD CONSTRAINT [PK_StorageBoxes]
    PRIMARY KEY CLUSTERED ([StorageBoxId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ComicComicsId] in table 'Authors'
ALTER TABLE [dbo].[Authors]
ADD CONSTRAINT [FK_ComicAuthor]
    FOREIGN KEY ([ComicComicsId])
    REFERENCES [dbo].[Comics]
        ([ComicsId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ComicAuthor'
CREATE INDEX [IX_FK_ComicAuthor]
ON [dbo].[Authors]
    ([ComicComicsId]);
GO

-- Creating foreign key on [ComicComicsId] in table 'Valuations'
ALTER TABLE [dbo].[Valuations]
ADD CONSTRAINT [FK_ComicValuation]
    FOREIGN KEY ([ComicComicsId])
    REFERENCES [dbo].[Comics]
        ([ComicsId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ComicValuation'
CREATE INDEX [IX_FK_ComicValuation]
ON [dbo].[Valuations]
    ([ComicComicsId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------