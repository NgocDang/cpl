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

-- 20180911-Lottery---
INSERT INTO LangDetail VALUES (1, N'CreateNewCategory', N'Create new category');
INSERT INTO LangDetail VALUES (2, N'CreateNewCategory', N'新しいカテゴリを作成する');

INSERT INTO LangDetail VALUES (1, N'ExistingCategory', N'Existing category, please use another.');
INSERT INTO LangDetail VALUES (2, N'ExistingCategory', N'既存のカテゴリは、別のカテゴリを使用してください。');

INSERT INTO LangDetail VALUES (1, N'LotteryCategory', N'Lottery Category');
INSERT INTO LangDetail VALUES (2, N'LotteryCategory', N'宝くじカテゴリ');

INSERT INTO LangDetail VALUES (1, N'LotteryCategoryRequired', N'Please select lottery category');
INSERT INTO LangDetail VALUES (2, N'LotteryCategoryRequired', N'抽選カテゴリを選択してください');

INSERT INTO LangDetail VALUES (1, N'LotteryCategoryNameRequired', N'Please fill in lottery category name');
INSERT INTO LangDetail VALUES (2, N'LotteryCategoryNameRequired', N'宝くじカテゴリ名を記入してください');
---------------------------------------------------------------------------------------------

-- 20180912- Admin Lottery --

INSERT INTO LangDetail VALUES (1, N'NumberOfTicket', N'Number Of Ticket');
INSERT INTO LangDetail VALUES (2, N'NumberOfTicket', N'チケットの数');

INSERT INTO LangDetail VALUES (1, N'TotalPurchasePrice', N'Total purchase price');
INSERT INTO LangDetail VALUES (2, N'TotalPurchasePrice', N'総購入価格');

INSERT INTO LangDetail VALUES (1, N'PurchaseDateTime', N'Purchase date time');
INSERT INTO LangDetail VALUES (2, N'PurchaseDateTime', N'購入日時');

INSERT INTO LangDetail VALUES (1, N'Details', N'Details');
INSERT INTO LangDetail VALUES (2, N'Details', N'詳細');

INSERT INTO LangDetail VALUES (1, N'DeactivateLotteryGameConfirmation', N'Deactivate lottery game confirmation');
INSERT INTO LangDetail VALUES (2, N'DeactivateLotteryGameConfirmation', N'宝くじゲームの確認を無効にする');

---------------------------------------------------------------------------------------------

-- 20180913- Affiliate View --
UPDATE LangDetail SET Value = N'Affiliate Generated URL' WHERE Name = N'AffliateUrl' and LangId = 1;

INSERT INTO LangDetail VALUES (1, N'AffliateUrlComment', N'Lost CPL Amount of users who registered using this link will become affiliate sales');
INSERT INTO LangDetail VALUES (2, N'AffliateUrlComment', N'このURLからユーザーが登録したユーザーの負け金が売上となります。');

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
INSERT INTO LangDetail VALUES (1, N'Sales', N'Sales');
INSERT INTO LangDetail VALUES (2, N'Sales', N'売上');

-- 20180914-AdminLotery---
INSERT INTO LangDetail VALUES (1, N'Revenue', N'Revenue');
INSERT INTO LangDetail VALUES (2, N'Revenue', N'収入');

INSERT INTO LangDetail VALUES (1, N'Device', N'Device');
INSERT INTO LangDetail VALUES (2, N'Device', N'デバイス');


INSERT INTO LangDetail VALUES (1, N'Desktop', N'Desktop');
INSERT INTO LangDetail VALUES (2, N'Desktop', N'デスクトップ');

INSERT INTO LangDetail VALUES (1, N'Tablet', N'Tablet');
INSERT INTO LangDetail VALUES (2, N'Tablet', N'タブレット');

INSERT INTO LangDetail VALUES (1, N'NoData', N'No Data Available');
INSERT INTO LangDetail VALUES (2, N'NoData', N'データなし')

-- 20180919-PricePredictionUpdate --
update LangDetail set Value = N'BTC/USDT' where name = N'BTCPricePredictionChartTitle'
update LangDetail set Value = N'Up', name = N'Up' where name = N'Up'
update LangDetail set Value = N'Down', name = N'Down' where name = N'Down'
update LangDetail set Value = N'今すぐ購入' where name = N'Bet' and value = 2
------------------------------------------------------------------------------------------