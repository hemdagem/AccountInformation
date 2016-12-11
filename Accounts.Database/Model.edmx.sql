
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/11/2016 21:33:26
-- Generated from EDMX file: C:\Code\account-info\Accounts.Database\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Account];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Payments_Income]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_Payments_Income];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Incomes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Incomes];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Incomes'
CREATE TABLE [dbo].[Incomes] (
    [Id] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Amount] decimal(19,4)  NULL,
    [PayDay] int  NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [Id] uniqueidentifier  NOT NULL,
    [IncomeId] uniqueidentifier  NOT NULL,
    [Date] datetime  NOT NULL,
    [Amount] decimal(19,4)  NOT NULL,
    [PaidYearly] bit  NOT NULL,
    [Recurring] bit  NOT NULL,
    [PaymentTitle] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Incomes'
ALTER TABLE [dbo].[Incomes]
ADD CONSTRAINT [PK_Incomes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [IncomeId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_Payments_Income]
    FOREIGN KEY ([IncomeId])
    REFERENCES [dbo].[Incomes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Payments_Income'
CREATE INDEX [IX_FK_Payments_Income]
ON [dbo].[Payments]
    ([IncomeId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------