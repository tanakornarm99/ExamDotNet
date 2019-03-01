USE [master]
GO
/****** Object:  Database [Example]    Script Date: 3/1/2019 20:42:52 ******/
CREATE DATABASE [Example]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Example', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Example.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Example_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\Example_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Example] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Example].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Example] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Example] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Example] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Example] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Example] SET ARITHABORT OFF 
GO
ALTER DATABASE [Example] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Example] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Example] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Example] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Example] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Example] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Example] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Example] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Example] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Example] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Example] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Example] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Example] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Example] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Example] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Example] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Example] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Example] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Example] SET  MULTI_USER 
GO
ALTER DATABASE [Example] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Example] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Example] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Example] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Example] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Example] SET QUERY_STORE = OFF
GO
USE [Example]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 3/1/2019 20:42:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 3/1/2019 20:42:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Firstname] [nvarchar](50) NOT NULL,
	[Surename] [nvarchar](50) NOT NULL,
	[Contact] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 3/1/2019 20:42:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Price] [numeric](10, 2) NULL,
	[CategoryId] [int] NULL,
 CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 3/1/2019 20:42:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderNumber] [nvarchar](50) NOT NULL,
	[From] [nvarchar](50) NOT NULL,
	[SummaryPrice] [numeric](18, 2) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderItem]    Script Date: 3/1/2019 20:42:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItem](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderId] [bigint] NOT NULL,
	[ItemName] [nvarchar](50) NOT NULL,
	[Category] [nvarchar](50) NOT NULL,
	[Qty] [int] NOT NULL,
	[Price] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([Id], [Name]) VALUES (17, N'Storage')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (18, N'Accessories')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (19, N'Memory')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (20, N'Electrical')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (22, N'Smart Phone')
INSERT [dbo].[Category] ([Id], [Name]) VALUES (23, N'Laptop')
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([Id], [Firstname], [Surename], [Contact], [Email]) VALUES (1, N'Mr.Tony', N'Smith', N'+6689425657', N'tony.smith@email.com')
SET IDENTITY_INSERT [dbo].[Customer] OFF
SET IDENTITY_INSERT [dbo].[Item] ON 

INSERT [dbo].[Item] ([Id], [Name], [Price], [CategoryId]) VALUES (1, N'HDD 2TB', CAST(2500.00 AS Numeric(10, 2)), 17)
INSERT [dbo].[Item] ([Id], [Name], [Price], [CategoryId]) VALUES (2, N'MicroSD Card 16GB', CAST(550.00 AS Numeric(10, 2)), 17)
INSERT [dbo].[Item] ([Id], [Name], [Price], [CategoryId]) VALUES (3, N'RAM DDR4 8GB', CAST(2500.00 AS Numeric(10, 2)), 19)
INSERT [dbo].[Item] ([Id], [Name], [Price], [CategoryId]) VALUES (4, N'LG UHD4K43"', CAST(12500.00 AS Numeric(10, 2)), 20)
INSERT [dbo].[Item] ([Id], [Name], [Price], [CategoryId]) VALUES (5, N'Iphone XS Max 256GB', CAST(45000.00 AS Numeric(10, 2)), 22)
INSERT [dbo].[Item] ([Id], [Name], [Price], [CategoryId]) VALUES (6, N'Mi notebook Pro 15.6', CAST(38500.00 AS Numeric(10, 2)), 23)
SET IDENTITY_INSERT [dbo].[Item] OFF
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([Id], [OrderNumber], [From], [SummaryPrice], [Date]) VALUES (19, N'ORD2019020001', N'Tanakorn Thuengfung', CAST(38500.00 AS Numeric(18, 2)), CAST(N'2019-02-24T17:32:01.000' AS DateTime))
INSERT [dbo].[Order] ([Id], [OrderNumber], [From], [SummaryPrice], [Date]) VALUES (20, N'ORD2019020002', N'tk arm99', CAST(12500.00 AS Numeric(18, 2)), CAST(N'2019-02-24T17:32:11.000' AS DateTime))
INSERT [dbo].[Order] ([Id], [OrderNumber], [From], [SummaryPrice], [Date]) VALUES (21, N'ORD2019020003', N'Numfah', CAST(45000.00 AS Numeric(18, 2)), CAST(N'2019-02-24T17:32:17.000' AS DateTime))
INSERT [dbo].[Order] ([Id], [OrderNumber], [From], [SummaryPrice], [Date]) VALUES (22, N'ORD2019020004', N'John', CAST(20550.00 AS Numeric(18, 2)), CAST(N'2019-03-01T17:21:10.000' AS DateTime))
INSERT [dbo].[Order] ([Id], [OrderNumber], [From], [SummaryPrice], [Date]) VALUES (23, N'ORD2019020005', N'Tony', CAST(0.00 AS Numeric(18, 2)), CAST(N'2019-03-01T17:23:38.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Order] OFF
SET IDENTITY_INSERT [dbo].[OrderItem] ON 

INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (70, 19, N'Mi notebook Pro 15.6', N'Laptop', 1, CAST(38500.00 AS Numeric(18, 2)))
INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (71, 20, N'LG UHD4K43&quot;', N'Electrical', 1, CAST(12500.00 AS Numeric(18, 2)))
INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (72, 21, N'Iphone XS Max 256GB', N'Smart Phone', 1, CAST(45000.00 AS Numeric(18, 2)))
INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (73, 22, N'HDD 2TB', N'Storage', 2, CAST(5000.00 AS Numeric(18, 2)))
INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (74, 22, N'MicroSD Card 16GB', N'Storage', 1, CAST(550.00 AS Numeric(18, 2)))
INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (75, 22, N'RAM DDR4 8GB', N'Memory', 1, CAST(2500.00 AS Numeric(18, 2)))
INSERT [dbo].[OrderItem] ([Id], [OrderId], [ItemName], [Category], [Qty], [Price]) VALUES (76, 22, N'LG UHD4K43&quot;', N'Electrical', 1, CAST(12500.00 AS Numeric(18, 2)))
SET IDENTITY_INSERT [dbo].[OrderItem] OFF
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Item] CHECK CONSTRAINT [FK_Item_Category]
GO
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderId])
REFERENCES [dbo].[Order] ([Id])
GO
ALTER TABLE [dbo].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO
USE [master]
GO
ALTER DATABASE [Example] SET  READ_WRITE 
GO
