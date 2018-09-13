-- Please use this file to add lang detail for your feature implementation

-- 20180911-LotteryGame---
ALTER TABLE LOTTERY
ADD IsDeleted BIT NOT NULL default(0)

INSERT INTO LangDetail VALUES (1, N'DeactivateSuccessfully', N'Deactivate Successfully !')
INSERT INTO LangDetail VALUES (2, N'DeactivateSuccessfully', N'正常に終了する!')

INSERT INTO LangDetail VALUES (1, N'Deactivated', N'Deactivated')
INSERT INTO LangDetail VALUES (2, N'Deactivated', N'無効化された')

INSERT INTO LangDetail VALUES (1, N'Deactivate', N'Deactivate')
INSERT INTO LangDetail VALUES (2, N'Deactivate', N'無効化する')
-------------------------------------------------------------------------------------------

-- 20180911-PricePrediction history---
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'CurrencyPair', N'Currency Pair')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'CurrencyPair', N'通貨ペア')
-------------------------------------------------------------------------------------------

-- 20180911-GameManagement---
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'Summary', N'Summary')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'Summary', N'概要')
-------------------------------------------------------------------------------------------

-- 20180913-GameManagement---
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'StatisticChart', N'Statistic Chart')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'StatisticChart', N'統計チャート')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'TotalRevenue', N'Total Revenue')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'TotalRevenue', N'総収入')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'TotalSale', N'Total Sale')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'TotalSale', N'トータルセール')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'PageView', N'Page View')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'PageView', N'ページビュー')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'TotalPlayers', N'Total Players')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'TotalPlayers', N'総プレーヤー')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'TodayPlayers', N'Today Players')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'TodayPlayers', N'今日のプレーヤー')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'TimeRange', N'Time Range')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'TimeRange', N'時間範囲')
-------------------------------------------------------------------------------------------