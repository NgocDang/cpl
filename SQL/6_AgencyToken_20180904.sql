USE CPL

INSERT INTO LangDetail VALUES (1, N'AgencyAffiliateURLGenerated', N'Agency affiliate url is generated successfully.');
INSERT INTO LangDetail VALUES (2, N'AgencyAffiliateURLGenerated', N'代理店のアフィリエイトのURLが正常に生成されます。');

INSERT INTO LangDetail VALUES (1, N'AgencyURLGenerator', N'Agency URL Generator');
INSERT INTO LangDetail VALUES (2, N'AgencyURLGenerator', N'代理店URLジェネレータ');

INSERT INTO LangDetail VALUES (1, N'AgencyURLGeneratorDesc', N'Please specify how many days that this URL will be expired.');
INSERT INTO LangDetail VALUES (2, N'AgencyURLGeneratorDesc', N'このURLの期限が切れる日数を指定してください。');

INSERT INTO LangDetail VALUES (1, N'ExpiredDayFromNow', N'Number of days from now');
INSERT INTO LangDetail VALUES (2, N'ExpiredDayFromNow', N'今からの日数');

INSERT INTO LangDetail VALUES (1, N'NumberOfDaysRequired', N'Please fill in number of days');
INSERT INTO LangDetail VALUES (2, N'NumberOfDaysRequired', N'日数を記入してください');

INSERT INTO LangDetail VALUES (1, N'Generate', N'Generate');
INSERT INTO LangDetail VALUES (2, N'Generate', N'生成する');

INSERT INTO LangDetail VALUES (1, N'UrlGenerated', N'URL generated');
INSERT INTO LangDetail VALUES (2, N'UrlGenerated', N'生成されたURL');


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