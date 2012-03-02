
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/23/2011 14:04:09
-- Generated from EDMX file: D:\Work\Products\ProductCode\TelChina.Platform\TelChina.TRF\ServiceDemo\TelChina.TRF.PersistentLayer\Model\DemoEDM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [DCM_Demo];
GO
IF SCHEMA_ID(N'Demo') IS NULL EXECUTE(N'CREATE SCHEMA [Demo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[Demo].[FK_OrderOrderLine]', 'F') IS NOT NULL
    ALTER TABLE [Demo].[OrderLineSet] DROP CONSTRAINT [FK_OrderOrderLine];
GO
IF OBJECT_ID(N'[Demo].[FK_UserUserRole]', 'F') IS NOT NULL
    ALTER TABLE [Demo].[UserRoleSet] DROP CONSTRAINT [FK_UserUserRole];
GO
IF OBJECT_ID(N'[Demo].[FK_RoleUserRole]', 'F') IS NOT NULL
    ALTER TABLE [Demo].[UserRoleSet] DROP CONSTRAINT [FK_RoleUserRole];
GO
IF OBJECT_ID(N'[Demo].[FK_MOPick]', 'F') IS NOT NULL
    ALTER TABLE [Demo].[PickSet] DROP CONSTRAINT [FK_MOPick];
GO
IF OBJECT_ID(N'[Demo].[FK_MO_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [Demo].[OrderSet_MO] DROP CONSTRAINT [FK_MO_inherits_Order];
GO
IF OBJECT_ID(N'[Demo].[FK_SO_inherits_Order]', 'F') IS NOT NULL
    ALTER TABLE [Demo].[OrderSet_SO] DROP CONSTRAINT [FK_SO_inherits_Order];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[Demo].[SysParam]', 'U') IS NOT NULL
    DROP TABLE [Demo].[SysParam];
GO
IF OBJECT_ID(N'[Demo].[OrderSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[OrderSet];
GO
IF OBJECT_ID(N'[Demo].[OrderLineSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[OrderLineSet];
GO
IF OBJECT_ID(N'[Demo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[UserSet];
GO
IF OBJECT_ID(N'[Demo].[RoleSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[RoleSet];
GO
IF OBJECT_ID(N'[Demo].[UserRoleSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[UserRoleSet];
GO
IF OBJECT_ID(N'[Demo].[DataTypeTestSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[DataTypeTestSet];
GO
IF OBJECT_ID(N'[Demo].[PickSet]', 'U') IS NOT NULL
    DROP TABLE [Demo].[PickSet];
GO
IF OBJECT_ID(N'[Demo].[OrderSet_MO]', 'U') IS NOT NULL
    DROP TABLE [Demo].[OrderSet_MO];
GO
IF OBJECT_ID(N'[Demo].[OrderSet_SO]', 'U') IS NOT NULL
    DROP TABLE [Demo].[OrderSet_SO];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SysParam'
CREATE TABLE [Demo].[SysParam] (
    [sysparamid] bigint IDENTITY(1,1) NOT NULL,
    [sysparamName] varchar(50)  NOT NULL,
    [sysparamDsc] varchar(50)  NOT NULL,
    [sysparamValue] varchar(100)  NOT NULL,
    [SysVersion] int  NOT NULL
);
GO

-- Creating table 'OrderSet'
CREATE TABLE [Demo].[OrderSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [DocNo] nvarchar(max)  NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] nvarchar(max)  NULL,
    [SysVersion] int  NOT NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NULL
);
GO

-- Creating table 'OrderLineSet'
CREATE TABLE [Demo].[OrderLineSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Detail] nvarchar(max)  NOT NULL,
    [OrderId] bigint  NOT NULL,
    [LineNum] int  NOT NULL,
    [SysVersion] bigint  NOT NULL
);
GO

-- Creating table 'UserSet'
CREATE TABLE [Demo].[UserSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] nvarchar(max)  NULL,
    [SysVersion] int  NOT NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RoleSet'
CREATE TABLE [Demo].[RoleSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] nvarchar(max)  NULL,
    [SysVersion] int  NOT NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserRoleSet'
CREATE TABLE [Demo].[UserRoleSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] nvarchar(max)  NULL,
    [SysVersion] int  NOT NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NULL,
    [UserId] bigint  NOT NULL,
    [RoleId] bigint  NOT NULL
);
GO

-- Creating table 'DataTypeTestSet'
CREATE TABLE [Demo].[DataTypeTestSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Property] varbinary(max)  NOT NULL,
    [Property_1] bit  NOT NULL,
    [Property_2] tinyint  NOT NULL,
    [Property_3] datetime  NOT NULL,
    [Property_4] datetimeoffset  NOT NULL,
    [Property_5] decimal(18,0)  NOT NULL,
    [Property_6] float  NOT NULL,
    [Property_7] uniqueidentifier  NOT NULL,
    [Property_8] smallint  NOT NULL,
    [Property_9] bigint  NOT NULL,
    [Property_11] real  NOT NULL
);
GO

-- Creating table 'PickSet'
CREATE TABLE [Demo].[PickSet] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [CreatedOn] datetime  NULL,
    [CreatedBy] nvarchar(max)  NULL,
    [SysVersion] int  NOT NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedOn] datetime  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [MOId] bigint  NOT NULL
);
GO

-- Creating table 'OrderSet_MO'
CREATE TABLE [Demo].[OrderSet_MO] (
    [Product] nvarchar(max)  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- Creating table 'OrderSet_SO'
CREATE TABLE [Demo].[OrderSet_SO] (
    [Customer] nvarchar(max)  NOT NULL,
    [Id] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [sysparamid] in table 'SysParam'
ALTER TABLE [Demo].[SysParam]
ADD CONSTRAINT [PK_SysParam]
    PRIMARY KEY CLUSTERED ([sysparamid] ASC);
GO

-- Creating primary key on [Id] in table 'OrderSet'
ALTER TABLE [Demo].[OrderSet]
ADD CONSTRAINT [PK_OrderSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderLineSet'
ALTER TABLE [Demo].[OrderLineSet]
ADD CONSTRAINT [PK_OrderLineSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [Demo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleSet'
ALTER TABLE [Demo].[RoleSet]
ADD CONSTRAINT [PK_RoleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserRoleSet'
ALTER TABLE [Demo].[UserRoleSet]
ADD CONSTRAINT [PK_UserRoleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DataTypeTestSet'
ALTER TABLE [Demo].[DataTypeTestSet]
ADD CONSTRAINT [PK_DataTypeTestSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PickSet'
ALTER TABLE [Demo].[PickSet]
ADD CONSTRAINT [PK_PickSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderSet_MO'
ALTER TABLE [Demo].[OrderSet_MO]
ADD CONSTRAINT [PK_OrderSet_MO]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderSet_SO'
ALTER TABLE [Demo].[OrderSet_SO]
ADD CONSTRAINT [PK_OrderSet_SO]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [OrderId] in table 'OrderLineSet'
ALTER TABLE [Demo].[OrderLineSet]
ADD CONSTRAINT [FK_OrderOrderLine]
    FOREIGN KEY ([OrderId])
    REFERENCES [Demo].[OrderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderOrderLine'
CREATE INDEX [IX_FK_OrderOrderLine]
ON [Demo].[OrderLineSet]
    ([OrderId]);
GO

-- Creating foreign key on [UserId] in table 'UserRoleSet'
ALTER TABLE [Demo].[UserRoleSet]
ADD CONSTRAINT [FK_UserUserRole]
    FOREIGN KEY ([UserId])
    REFERENCES [Demo].[UserSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserUserRole'
CREATE INDEX [IX_FK_UserUserRole]
ON [Demo].[UserRoleSet]
    ([UserId]);
GO

-- Creating foreign key on [RoleId] in table 'UserRoleSet'
ALTER TABLE [Demo].[UserRoleSet]
ADD CONSTRAINT [FK_RoleUserRole]
    FOREIGN KEY ([RoleId])
    REFERENCES [Demo].[RoleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUserRole'
CREATE INDEX [IX_FK_RoleUserRole]
ON [Demo].[UserRoleSet]
    ([RoleId]);
GO

-- Creating foreign key on [MOId] in table 'PickSet'
ALTER TABLE [Demo].[PickSet]
ADD CONSTRAINT [FK_MOPick]
    FOREIGN KEY ([MOId])
    REFERENCES [Demo].[OrderSet_MO]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MOPick'
CREATE INDEX [IX_FK_MOPick]
ON [Demo].[PickSet]
    ([MOId]);
GO

-- Creating foreign key on [Id] in table 'OrderSet_MO'
ALTER TABLE [Demo].[OrderSet_MO]
ADD CONSTRAINT [FK_MO_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [Demo].[OrderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'OrderSet_SO'
ALTER TABLE [Demo].[OrderSet_SO]
ADD CONSTRAINT [FK_SO_inherits_Order]
    FOREIGN KEY ([Id])
    REFERENCES [Demo].[OrderSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------