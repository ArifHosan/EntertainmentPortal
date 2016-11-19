
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/18/2016 18:27:14
-- Generated from EDMX file: F:\Education Stuff\Projects\OOP2\AsyncExample\MoviePortal.v1.1\MoviePortal.v1.1\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MovieFound];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Entity1'
CREATE TABLE [dbo].[Entity1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [Year] nvarchar(max)  NOT NULL,
    [Rated] nvarchar(max)  NOT NULL,
    [Released] nvarchar(max)  NOT NULL,
    [Runtime] nvarchar(max)  NOT NULL,
    [Genre] nvarchar(max)  NOT NULL,
    [Director] nvarchar(max)  NOT NULL,
    [Writer] nvarchar(max)  NOT NULL,
    [Actors] nvarchar(max)  NOT NULL,
    [Plot] nvarchar(max)  NOT NULL,
    [Language] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NOT NULL,
    [Awards] nvarchar(max)  NOT NULL,
    [Poster] nvarchar(max)  NOT NULL,
    [MetaScore] nvarchar(max)  NOT NULL,
    [ImdbRating] nvarchar(max)  NOT NULL,
    [ImdbVotes] nvarchar(max)  NOT NULL,
    [imdbId] nvarchar(max)  NOT NULL,
    [Type] nvarchar(max)  NOT NULL,
    [TomatoMeter] nvarchar(max)  NOT NULL,
    [TomatoImage] nvarchar(max)  NOT NULL,
    [TomatoRating] nvarchar(max)  NOT NULL,
    [TomatoUserMeter] nvarchar(max)  NOT NULL,
    [TomatoUserRating] nvarchar(max)  NOT NULL,
    [TomatoUserReviews] nvarchar(max)  NOT NULL,
    [TomatoReviews] nvarchar(max)  NOT NULL,
    [TomatoFresh] nvarchar(max)  NOT NULL,
    [TomatoRotten] nvarchar(max)  NOT NULL,
    [TomatoConsensus] nvarchar(max)  NOT NULL,
    [TomatoURL] nvarchar(max)  NOT NULL,
    [DVD] nvarchar(max)  NOT NULL,
    [BoxOffice] nvarchar(max)  NOT NULL,
    [Production] nvarchar(max)  NOT NULL,
    [Website] nvarchar(max)  NOT NULL,
    [Response] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Entity1'
ALTER TABLE [dbo].[Entity1]
ADD CONSTRAINT [PK_Entity1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------