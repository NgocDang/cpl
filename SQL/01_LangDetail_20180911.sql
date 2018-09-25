-- Please use this file to add lang detail for your feature implementation
use CPL;

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
update LangDetail set Value = N'High', name = N'High' where name = N'Up'
update LangDetail set Value = N'Low', name = N'Low' where name = N'Down'
update LangDetail set Value = N'今すぐ購入' where name = N'Bet' and LangId = 2
------------------------------------------------------------------------------------------


-- 20180919-AdminTopAgency---

INSERT INTO LangDetail VALUES (1, N'Pay', N'Pay');
INSERT INTO LangDetail VALUES (2, N'Pay', N'支払う');

INSERT INTO LangDetail VALUES (1, N'IsTier2TabVisible', N'Is tier2 tab visible');
INSERT INTO LangDetail VALUES (2, N'IsTier2TabVisible', N'Tier2タブが可視かどうか');

INSERT INTO LangDetail VALUES (1, N'IsTier3TabVisible', N'Is tier3 tab visible');
INSERT INTO LangDetail VALUES (2, N'IsTier3TabVisible', N'Tier3タブが可視かどうか');

INSERT INTO LangDetail VALUES (1, N'IsAutoPaymentEnable', N'Is auto payment enable');
INSERT INTO LangDetail VALUES (2, N'IsAutoPaymentEnable', N'自動支払いが有効かどうか');

INSERT INTO LangDetail VALUES (1, N'Payment', N'Payment');
INSERT INTO LangDetail VALUES (2, N'Payment', N'支払い');

INSERT INTO LangDetail VALUES (1, N'ConfirmPayment', N'Do you want to pay commision ?');
INSERT INTO LangDetail VALUES (2, N'ConfirmPayment', N'手数料を払いたいですか？');

INSERT INTO LangDetail VALUES (1, N'CommissionAmount', N'Commission amount');
INSERT INTO LangDetail VALUES (2, N'CommissionAmount', N'手数料額');

INSERT INTO LangDetail VALUES (1, N'PeriodInMonth', N'Period in month');
INSERT INTO LangDetail VALUES (2, N'PeriodInMonth', N'月の期間');

INSERT INTO LangDetail VALUES (1, N'PaidSuccessfully', N'Paid successfully !');
INSERT INTO LangDetail VALUES (2, N'PaidSuccessfully', N'支払いは正常に完了しました');

INSERT INTO LangDetail VALUES (1, N'TopAgencyAffiliate', N'Top Agency Affiliate');
INSERT INTO LangDetail VALUES (2, N'TopAgencyAffiliate', N'トップの代理店');

----------------------------------------------------------------------------------

--20180920-AdminTopAgency--
INSERT INTO LangDetail VALUES (1, N'DirectSale', N'Direct Sale');
INSERT INTO LangDetail VALUES (2, N'DirectSale', N'直販');

INSERT INTO LangDetail VALUES (1, N'TotalIntroducedUsers', N'Total Introduced Users');
INSERT INTO LangDetail VALUES (2, N'TotalIntroducedUsers', N'総導入ユーザー数');

INSERT INTO LangDetail VALUES (1, N'DirectIntroducedUsers', N'Direct Introduced Users');
INSERT INTO LangDetail VALUES (2, N'DirectIntroducedUsers', N'直接導入されたユーザー');

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'TopAgency', N'Top Agency')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'TopAgency', N'トップエージェンシー')
---------------------------------------------------------------------------------
------------------------------------------------------------------------------------------

--20180921----
INSERT INTO LangDetail VALUES (1, N'OverBettingTime', N'Betting time for this game is over');
INSERT INTO LangDetail VALUES (2, N'OverBettingTime', N'このゲームの賭けの時間は終わった');

-----------------------------------------------------------------------------------------
-- LAN - 20180920 - LotteryCategory card in Admin -> GameManagement -> Lottery tab ------

INSERT INTO LangDetail VALUES (1, N'AddANewLotteryCategory', N'Add new lottery category');
INSERT INTO LangDetail VALUES (2, N'AddANewLotteryCategory', N'新しい宝くじカテゴリを追加');

--20180920-AdminAddTopAgency---
INSERT INTO LangDetail VALUES (1, N'ListOfTopAgencyAffiliate', N'List of top agency affiliate');
INSERT INTO LangDetail VALUES (2, N'ListOfTopAgencyAffiliate', N'トップ代理店アフィリエイトのリスト');

INSERT INTO LangDetail VALUES (1, N'TopAgencyAffiliateTiers', N'Top agency affiliate tiers');
INSERT INTO LangDetail VALUES (2, N'TopAgencyAffiliateTiers', N'上位機関のアフィリエイト層');

INSERT INTO LangDetail VALUES (1, N'AllTopAgencyAffiliate', N'All top agency affiliate');
INSERT INTO LangDetail VALUES (2, N'AllTopAgencyAffiliate', N'すべてのトップ代理店アフィリエイト');

DELETE LangDetail WHERE Name = 'TotalIntroducer';
-----------------------------------------------------------------------------------------

--20180924--StandardAffiliate for Admin-------------------------------------------------
INSERT INTO LangDetail VALUES (1, N'AllStandardAffiliate', N'All Standard Affiliate');
INSERT INTO LangDetail VALUES (2, N'AllStandardAffiliate', N'すべての標準アフィリエイト');

INSERT INTO LangDetail VALUES (1, N'TotalCPLLost', N'Total CPL lost');
INSERT INTO LangDetail VALUES (2, N'TotalCPLLost', N'紛失総CPL');

INSERT INTO LangDetail VALUES (1, N'KindOfTier', N'Kind of tier');
INSERT INTO LangDetail VALUES (2, N'KindOfTier', N'層の種類');
