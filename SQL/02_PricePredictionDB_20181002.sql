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
	[Name] [nvarchar](200) NULL,
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

-- SAMPLE DB
USE [CPL]
GO
SET IDENTITY_INSERT [dbo].[PricePredictionCategory] ON 

INSERT [dbo].[PricePredictionCategory] ([Id]) VALUES (1)
SET IDENTITY_INSERT [dbo].[PricePredictionCategory] OFF
SET IDENTITY_INSERT [dbo].[PricePredictionCategoryDetail] ON 

INSERT [dbo].[PricePredictionCategoryDetail] ([Id], [Name], [Description], [PricePredictionCategoryId], [LangId]) VALUES (1, N'Price Prediction 1', NULL, 1, 1)
INSERT [dbo].[PricePredictionCategoryDetail] ([Id], [Name], [Description], [PricePredictionCategoryId], [LangId]) VALUES (2, N'価格予測1', NULL, 1, 2)
SET IDENTITY_INSERT [dbo].[PricePredictionCategoryDetail] OFF
SET IDENTITY_INSERT [dbo].[PricePredictionSetting] ON 

INSERT [dbo].[PricePredictionSetting] ([Id], [OpenBettingTime], [CloseBettingTime], [HoldingTimeInterval], [ResultTimeInterval], [DividedRate], [CreatedDate], [PricePredictionCategoryId]) VALUES (1, CAST(N'2018-10-08T01:15:00.000' AS DateTime), CAST(N'2018-10-08T22:45:00.000' AS DateTime), 1, 15, 80, CAST(N'2018-10-08T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[PricePredictionSetting] ([Id], [OpenBettingTime], [CloseBettingTime], [HoldingTimeInterval], [ResultTimeInterval], [DividedRate], [CreatedDate], [PricePredictionCategoryId]) VALUES (2, CAST(N'2018-10-08T09:15:00.000' AS DateTime), CAST(N'2018-10-09T08:00:00.000' AS DateTime), 1, 15, 80, CAST(N'2018-10-08T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[PricePredictionSetting] ([Id], [OpenBettingTime], [CloseBettingTime], [HoldingTimeInterval], [ResultTimeInterval], [DividedRate], [CreatedDate], [PricePredictionCategoryId]) VALUES (3, CAST(N'2018-10-08T17:15:00.000' AS DateTime), CAST(N'2018-10-09T16:00:00.000' AS DateTime), 1, 15, 80, CAST(N'2018-10-08T00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[PricePredictionSetting] OFF
SET IDENTITY_INSERT [dbo].[PricePredictionSettingDetail] ON 

INSERT [dbo].[PricePredictionSettingDetail] ([Id], [Title], [LangId], [PricePredictionSettingId]) VALUES (1, N'Daily Game 01:15', 1, 1)
INSERT [dbo].[PricePredictionSettingDetail] ([Id], [Title], [LangId], [PricePredictionSettingId]) VALUES (2, N'Daily Game 01:15', 2, 1)
INSERT [dbo].[PricePredictionSettingDetail] ([Id], [Title], [LangId], [PricePredictionSettingId]) VALUES (3, N'Daily Game 09:15', 1, 2)
INSERT [dbo].[PricePredictionSettingDetail] ([Id], [Title], [LangId], [PricePredictionSettingId]) VALUES (4, N'Daily Game 09:15', 2, 2)
INSERT [dbo].[PricePredictionSettingDetail] ([Id], [Title], [LangId], [PricePredictionSettingId]) VALUES (5, N'Daily Game 17:15', 1, 3)
INSERT [dbo].[PricePredictionSettingDetail] ([Id], [Title], [LangId], [PricePredictionSettingId]) VALUES (6, N'Daily Game 17:15', 2, 3)
SET IDENTITY_INSERT [dbo].[PricePredictionSettingDetail] OFF



