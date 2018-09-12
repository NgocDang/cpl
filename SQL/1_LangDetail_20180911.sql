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

