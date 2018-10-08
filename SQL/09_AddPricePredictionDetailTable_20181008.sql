USE [CPL]
GO
/****** Object:  Table [dbo].[PricePredictionDetail]    Script Date: 10/8/2018 1:02:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PricePredictionDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[ShortDescription] [nvarchar](500) NULL,
	[LangId] [int] NOT NULL,
	[PricePredictionId] [int] NOT NULL,
 CONSTRAINT [PK_PricePredictionDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

USE CPL;
ALTER TABLE PricePrediction 
ADD PricePredictionCategoryId int not null default(0)

