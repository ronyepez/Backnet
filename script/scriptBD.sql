USE [master]
GO

/****** CREAR BASE DE DATOS ******/
CREATE DATABASE BackNet
GO

/****** CREAR TABLA stores ******/
USE [BackNet]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[stores](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](250) NOT NULL,
	[address] [varchar](300) NOT NULL,
 CONSTRAINT [PK_stores] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** CREAR TABLA articles ******/
USE [BackNet]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[articles](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](250) NOT NULL,
	[description] [varchar](500) NOT NULL,
	[price] [float] NOT NULL,
	[total_in_shelf] [int] NOT NULL,
	[total_in_vault] [int] NOT NULL,
	[store_id] [int] NOT NULL,
 CONSTRAINT [PK_articles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[articles]  WITH CHECK ADD  CONSTRAINT [FK_articles_stores] FOREIGN KEY([store_id])
REFERENCES [dbo].[stores] ([id])
GO

ALTER TABLE [dbo].[articles] CHECK CONSTRAINT [FK_articles_stores]
GO