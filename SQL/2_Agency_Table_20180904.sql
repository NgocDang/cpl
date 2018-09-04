USE [CPL]
GO
/****** Object:  Table [dbo].[Affiliate]    Script Date: 04/09/2018 3:04:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Affiliate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tier1DirectRate] [int] NOT NULL,
	[Tier2SaleToTier1Rate] [int] NOT NULL,
	[Tier3SaleToTier1Rate] [int] NOT NULL,
 CONSTRAINT [PK_Affiliate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


GO
/****** Object:  Table [dbo].[Agency]    Script Date: 04/09/2018 3:04:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Agency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Tier1DirectRate] [int] NOT NULL,
	[Tier2DirectRate] [int] NOT NULL,
	[Tier3DirectRate] [int] NOT NULL,
	[Tier2SaleToTier1Rate] [int] NOT NULL,
	[Tier3SaleToTier1Rate] [int] NOT NULL,
	[Tier3SaleToTier2Rate] [int] NOT NULL,
	[IsAutoPaymentEnable] [bit] NOT NULL,
	[IsTier2TabVisible] [bit] NOT NULL,
	[IsTier3TabVisible] [bit] NOT NULL,
 CONSTRAINT [PK_Agency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

alter table SysUser add  AgencyId int null;
alter table SysUser add  AffiliateId int null;
alter table SysUser add  IsIntroducedById int null;
alter table SysUser add AffiliateCreatedDate datetime null;