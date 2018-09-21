USE [CPL]
GO

/****** Object:  Table [dbo].[Payment]    Script Date: 17/09/2018 7:28:53 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Payment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SysUserId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[Tier1DirectSale] [money] NOT NULL,
	[Tier2SaleToTier1Sale] [money] NOT NULL,
	[Tier3SaleToTier1Sale] [money] NOT NULL,
	[Tier1DirectRate] [int] NOT NULL,
	[Tier2SaleToTier1Rate] [int] NOT NULL,
	[Tier3SaleToTier1Rate] [int] NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


