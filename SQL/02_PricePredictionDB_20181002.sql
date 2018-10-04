-- STRUCTURE

USE [CPL]
GO
/****** Object:  Table [dbo].[PricePredictionCategory]    Script Date: 10/2/2018 10:46:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PricePredictionCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_PricePredictionCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PricePredictionCategoryDetail]    Script Date: 10/2/2018 10:46:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PricePredictionCategoryDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [ntext] NULL,
	[PricePredictionCategoryId] [int] NOT NULL,
	[LangId] [int] NOT NULL,
 CONSTRAINT [PK_PricePredictionCategoryDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PricePredictionSetting]    Script Date: 10/2/2018 10:46:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PricePredictionSetting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OpenBettingTime] [datetime] NOT NULL,
	[CloseBettingTime] [datetime] NOT NULL,
	[HoldingTimeInterval] [int] NOT NULL,
	[ResultTimeInterval] [int] NOT NULL,
	[DividedRate] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[PricePredictionCategoryId] [int] NOT NULL,
 CONSTRAINT [PK_PricePredictionSetting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PricePredictionSettingDetail]    Script Date: 10/2/2018 10:46:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PricePredictionSettingDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[ShortDescription] [nvarchar](500) NULL,
	[LangId] [int] NOT NULL,
	[PricePredictionSettingId] [int] NOT NULL,
 CONSTRAINT [PK_PricePredictionSettingDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE CPL;
ALTER TABLE PricePrediction
ADD PricePredictionSettingId INT NOT NULL DEFAULT (0);
GO
UPDATE PricePrediction SET PricePredictionSettingId = 1
