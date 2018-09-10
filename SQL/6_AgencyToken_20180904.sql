USE CPL



GO
/****** Object:  Table [dbo].[AgencyToken]    Script Date: 9/4/2018 1:31:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgencyToken](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](50) NOT NULL,
	[ExpiredDate] [datetime] NOT NULL,
	[SysUserId] [int] NULL,
 CONSTRAINT [PK_AgencyToken] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO Setting VALUES (N'NumberOfAgencyAffiliateExpiredDays', 10, NULL);