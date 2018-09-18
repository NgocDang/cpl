use CPL;
truncate table [Lottery];
truncate table [LotteryDetail];
truncate table [LotteryHistory];
truncate table [LotteryPrize];
truncate table [LotteryCategory]
truncate table [PricePrediction];
truncate table [PricePredictionHistory];
truncate table [SysUser];
truncate table [Affiliate];
truncate table [Agency];

USE [CPL]
GO
SET IDENTITY_INSERT [dbo].[Affiliate] ON 
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (1, 1, 2, 3, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (2, 2, 2, 2, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (3, 3, 3, 3, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (4, 4, 4, 4, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (5, 5, 5, 5, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (6, 6, 6, 6, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (7, 7, 7, 7, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (8, 8, 8, 8, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (9, 9, 9, 9, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (10, 3, 2, 1, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (11, 2, 5, 1, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (12, 5, 7, 2, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (13, 4, 7, 5, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (14, 2, 5, 7, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (15, 8, 6, 11, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (16, 3, 5, 7, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (17, 7, 2, 4, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (18, 5, 5, 4, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (19, 2, 8, 9, 0, 0, 0)
GO
INSERT [dbo].[Affiliate] ([Id], [Tier1DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (20, 3, 7, 9, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Affiliate] OFF
GO
SET IDENTITY_INSERT [dbo].[Agency] ON 
GO
INSERT [dbo].[Agency] ([Id], [Tier1DirectRate], [Tier2DirectRate], [Tier3DirectRate], [Tier2SaleToTier1Rate], [Tier3SaleToTier1Rate], [Tier3SaleToTier2Rate], [IsAutoPaymentEnable], [IsTier2TabVisible], [IsTier3TabVisible]) VALUES (2, 1, 2, 3, 4, 5, 6, 0, 0, 0)
GO
SET IDENTITY_INSERT [dbo].[Agency] OFF
GO
SET IDENTITY_INSERT [dbo].[Lottery] ON 
GO
INSERT [dbo].[Lottery] ([Id], [Phase], [CreatedDate], [Volume], [Status], [Title], [UnitPrice], [UpdatedDate], [IsDeleted], [LotteryCategoryId]) VALUES (1, 1, CAST(N'2018-08-08T20:22:42.890' AS DateTime), 5000, 3, N'1BTC宝くじ', 500, CAST(N'2018-08-16T00:00:00.000' AS DateTime), 0, 1)
GO
INSERT [dbo].[Lottery] ([Id], [Phase], [CreatedDate], [Volume], [Status], [Title], [UnitPrice], [UpdatedDate], [IsDeleted], [LotteryCategoryId]) VALUES (2, 2, CAST(N'2018-08-08T20:26:40.453' AS DateTime), 5000, 3, N'2BTC宝くじ', 1000, CAST(N'2018-08-17T00:00:00.000' AS DateTime), 0, 1)
GO
INSERT [dbo].[Lottery] ([Id], [Phase], [CreatedDate], [Volume], [Status], [Title], [UnitPrice], [UpdatedDate], [IsDeleted], [LotteryCategoryId]) VALUES (3, 3, CAST(N'2018-08-08T20:31:25.240' AS DateTime), 5000, 2, N'0.5BTC宝くじ', 300, NULL, 0, 1)
GO
INSERT [dbo].[Lottery] ([Id], [Phase], [CreatedDate], [Volume], [Status], [Title], [UnitPrice], [UpdatedDate], [IsDeleted], [LotteryCategoryId]) VALUES (4, 4, CAST(N'2018-08-08T20:33:58.593' AS DateTime), 5000, 2, N'1BTC宝くじ', 300, NULL, 0, 1)
GO
SET IDENTITY_INSERT [dbo].[Lottery] OFF
GO
SET IDENTITY_INSERT [dbo].[LotteryCategory] ON 
GO
INSERT [dbo].[LotteryCategory] ([Id], [Name], [Description]) VALUES (1, N'Lottery 1', NULL)
GO
SET IDENTITY_INSERT [dbo].[LotteryCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[LotteryDetail] ON 
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (1, 1, 1, N'1_ds_20180828082242_desktop slider.jpg', N'1_dl_20180828082242_thumb-game_08.jpg', N'1_ml_20180828082242_thumb-game_17.jpg', N'1_ms_20180828082242_mobile banner.jpg', N'1_p_20180828082242_prize.jpg', N'<p>When the number of purchased lottery tickets reach the upper limit, we will make a lottery by Smart Contract.​</p>
<p>Lottery will be held at 0:00 am in Japan time and will be announced on the user''s management screen at 10 AM in the next morning.​</p>
<p>If the number of purchases does not reach the upper limit, it will be carried over to the next day until the upper ​limit is reached.</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (2, 2, 1, N'2_ds_20180828082640_desktop slider.jpg', N'2_dl_20180828082640_thumb-game_08.jpg', N'2_ml_20180828082640_thumb-game_17.jpg', N'2_ms_20180828082640_mobile banner.jpg', N'2_p_20180828082640_prize.jpg', N'<p>When the number of purchased lottery tickets reach the upper limit, we will make a lottery by Smart Contract.​</p>
<p>Lottery will be held at 0:00 am in Japan time and will be announced on the user''s management screen at 10 AM in the next morning.​</p>
<p>If the number of purchases does not reach the upper limit, it will be carried over to the next day until the upper ​limit is reached.</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (3, 3, 1, N'3_ds_20180828083125_desktop slider.jpg', N'3_dl_20180828083125_thumb-game_08.jpg', N'3_ml_20180828083125_thumb-game_17.jpg', N'3_ms_20180828083125_mobile banner.jpg', N'3_p_20180828083125_prize.jpg', N'<p>When the number of purchased lottery tickets reach the upper limit, we will make a lottery by Smart Contract.​</p>
<p>Lottery will be held at 0:00 am in Japan time and will be announced on the user''s management screen at 10 AM in the next morning.​</p>
<p>If the number of purchases does not reach the upper limit, it will be carried over to the next day until the upper ​limit is reached.</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (4, 4, 1, N'4_ds_20180828083358_desktop slider.jpg', N'4_dl_20180828083358_thumb-game_08.jpg', N'4_ml_20180828083358_thumb-game_17.jpg', N'4_ms_20180828083358_mobile banner.jpg', N'4_p_20180828083358_prize.jpg', N'<p>When the number of purchased lottery tickets reach the upper limit, we will make a lottery by Smart Contract.​</p>
<p>Lottery will be held at 0:00 am in Japan time and will be announced on the user''s management screen at 10 AM in the next morning.​</p>
<p>If the number of purchases does not reach the upper limit, it will be carried over to the next day until the upper ​limit is reached.</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (5, 1, 2, N'1_ds_20180828082242_desktop slider.jpg', N'1_dl_20180828082242_thumb-game_08.jpg', N'1_ml_20180828082242_thumb-game_17.jpg', N'1_ms_20180828082242_mobile banner.jpg', N'1_p_20180828082242_prize.jpg', N'<p>仮想通貨くじの購入数が上限に達した段階で、スマートコントラクトによる抽選を行います。​</p>  <p>抽選時間は日本時間午前0時に行われ、翌朝10時にユーザー様の管理画面上で発表されます。​</p>  <p>（購入数が上限に達しない場合は、上限に達するまで翌日に持ち越されます。)</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (6, 2, 2, N'2_ds_20180828082640_desktop slider.jpg', N'2_dl_20180828082640_thumb-game_08.jpg', N'2_ml_20180828082640_thumb-game_17.jpg', N'2_ms_20180828082640_mobile banner.jpg', N'2_p_20180828082640_prize.jpg', N'<p>仮想通貨くじの購入数が上限に達した段階で、スマートコントラクトによる抽選を行います。​</p>  <p>抽選時間は日本時間午前0時に行われ、翌朝10時にユーザー様の管理画面上で発表されます。​</p>  <p>（購入数が上限に達しない場合は、上限に達するまで翌日に持ち越されます。)</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (7, 3, 2, N'3_ds_20180828083125_desktop slider.jpg', N'3_dl_20180828083125_thumb-game_08.jpg', N'3_ml_20180828083125_thumb-game_17.jpg', N'3_ms_20180828083125_mobile banner.jpg', N'3_p_20180828083125_prize.jpg', N'<p>仮想通貨くじの購入数が上限に達した段階で、スマートコントラクトによる抽選を行います。​</p>  <p>抽選時間は日本時間午前0時に行われ、翌朝10時にユーザー様の管理画面上で発表されます。​</p>  <p>（購入数が上限に達しない場合は、上限に達するまで翌日に持ち越されます。)</p>')
GO
INSERT [dbo].[LotteryDetail] ([Id], [LotteryId], [LangId], [DesktopTopImage], [DesktopListingImage], [MobileListingImage], [MobileTopImage], [PrizeImage], [Description]) VALUES (8, 4, 2, N'4_ds_20180828083358_desktop slider.jpg', N'4_dl_20180828083358_thumb-game_08.jpg', N'4_ml_20180828083358_thumb-game_17.jpg', N'4_ms_20180828083358_mobile banner.jpg', N'4_p_20180828083358_prize.jpg', N'<p>仮想通貨くじの購入数が上限に達した段階で、スマートコントラクトによる抽選を行います。​</p>  <p>抽選時間は日本時間午前0時に行われ、翌朝10時にユーザー様の管理画面上で発表されます。​</p>  <p>（購入数が上限に達しない場合は、上限に達するまで翌日に持ち越されます。)</p>')
GO
SET IDENTITY_INSERT [dbo].[LotteryDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[LotteryHistory] ON 
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (1, 1, 1, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'111111', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 1, 1, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (2, 1, 2, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'222222', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 2, 2, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (3, 1, 3, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'333333', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 3, 3, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (5, 2, 4, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'444444', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 7, 4, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (6, 1, 2, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'555555', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 5, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (7, 2, 4, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 6, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (8, 2, 5, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'777777', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 6, 7, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (9, 2, 6, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'888888', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 8, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (10, 2, 7, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'999999', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 9, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (11, 2, 8, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'100000', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 10, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (12, 2, 8, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'111111', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 4, 11, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (13, 2, 9, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 12, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (14, 1, 10, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 13, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (15, 2, 11, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 4, 14, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (16, 1, 12, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 4, 15, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (17, 1, 13, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 16, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (18, 2, 14, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 17, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (19, 2, 14, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 5, 18, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (20, 2, 14, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 19, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (21, 2, 15, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 20, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (22, 2, 16, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), NULL, 21, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (23, 2, 17, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 8, 22, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (24, 2, 17, N'WIN', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 8, 23, N'')
GO
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex], [TxHashId]) VALUES (25, 2, 17, N'LOSE', CAST(N'2018-08-12T01:00:00.000' AS DateTime), N'666666', CAST(N'2018-08-13T00:00:00.000' AS DateTime), 8, 24, N'')
GO
SET IDENTITY_INSERT [dbo].[LotteryHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[LotteryPrize] ON 
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (1, 1000000.0000, 1, 1, 1)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (2, 100000.0000, 1, 5, 2)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (3, 10000.0000, 1, 25, 3)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (4, 500.0000, 1, 500, 4)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (5, 2000000.0000, 2, 1, 1)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (6, 200000.0000, 2, 5, 2)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (7, 20000.0000, 2, 40, 3)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (8, 1000.0000, 2, 200, 4)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (9, 500000.0000, 3, 1, 1)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (10, 50000.0000, 3, 10, 2)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (11, 5000.0000, 3, 25, 3)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (12, 300.0000, 3, 500, 4)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (13, 1000000.0000, 4, 1, 1)
GO
INSERT [dbo].[LotteryPrize] ([Id], [Value], [LotteryId], [Volume], [Index]) VALUES (14, 300.0000, 4, 500, 2)
GO
SET IDENTITY_INSERT [dbo].[LotteryPrize] OFF
GO
SET IDENTITY_INSERT [dbo].[PricePrediction] ON 
GO
INSERT [dbo].[PricePrediction] ([Id], [Name], [Description], [ResultPrice], [NumberOfPredictors], [Volume], [Coinbase], [UpdatedDate], [OpenBettingTime], [CloseBettingTime], [ResultTime], [ToBeComparedTime], [ToBeComparedPrice]) VALUES (1, N'Price Prediction #1', NULL, 8000.0000, 10, 10.0000, N'BTCUSDT', CAST(N'2018-08-13T09:15:00.000' AS DateTime), CAST(N'2018-08-12T09:16:00.000' AS DateTime), CAST(N'2018-08-13T08:00:00.000' AS DateTime), CAST(N'2018-08-13T09:15:00.000' AS DateTime), CAST(N'2018-08-13T09:00:00.000' AS DateTime), 7900.0000)
GO
INSERT [dbo].[PricePrediction] ([Id], [Name], [Description], [ResultPrice], [NumberOfPredictors], [Volume], [Coinbase], [UpdatedDate], [OpenBettingTime], [CloseBettingTime], [ResultTime], [ToBeComparedTime], [ToBeComparedPrice]) VALUES (2, N'Price Prediction #2', NULL, 8001.0000, 11, 11.0000, N'BTCUSDT', CAST(N'2018-08-13T09:15:00.000' AS DateTime), CAST(N'2018-08-12T17:16:00.000' AS DateTime), CAST(N'2018-08-13T16:00:00.000' AS DateTime), CAST(N'2018-08-13T17:15:00.000' AS DateTime), CAST(N'2018-08-13T17:00:00.000' AS DateTime), 7901.0000)
GO
INSERT [dbo].[PricePrediction] ([Id], [Name], [Description], [ResultPrice], [NumberOfPredictors], [Volume], [Coinbase], [UpdatedDate], [OpenBettingTime], [CloseBettingTime], [ResultTime], [ToBeComparedTime], [ToBeComparedPrice]) VALUES (3, N'Price Prediction #3', NULL, 8002.0000, 12, 12.0000, N'BTCUSDT', CAST(N'2018-08-13T09:15:00.000' AS DateTime), CAST(N'2018-08-13T01:16:00.000' AS DateTime), CAST(N'2018-08-14T00:00:00.000' AS DateTime), CAST(N'2018-08-14T01:15:00.000' AS DateTime), CAST(N'2018-08-14T01:00:00.000' AS DateTime), 7902.0000)
GO
INSERT [dbo].[PricePrediction] ([Id], [Name], [Description], [ResultPrice], [NumberOfPredictors], [Volume], [Coinbase], [UpdatedDate], [OpenBettingTime], [CloseBettingTime], [ResultTime], [ToBeComparedTime], [ToBeComparedPrice]) VALUES (4, N'Price Prediction #4', NULL, 8003.0000, 13, 13.0000, N'BTCUSDT', NULL, CAST(N'2018-08-13T09:16:00.000' AS DateTime), CAST(N'2018-08-14T08:00:00.000' AS DateTime), CAST(N'2018-08-14T09:15:00.000' AS DateTime), CAST(N'2018-08-14T09:00:00.000' AS DateTime), 7903.0000)
GO
INSERT [dbo].[PricePrediction] ([Id], [Name], [Description], [ResultPrice], [NumberOfPredictors], [Volume], [Coinbase], [UpdatedDate], [OpenBettingTime], [CloseBettingTime], [ResultTime], [ToBeComparedTime], [ToBeComparedPrice]) VALUES (5, N'Price Prediction #5', NULL, 8004.0000, 14, 14.0000, N'BTCUSDT', NULL, CAST(N'2018-08-13T17:16:00.000' AS DateTime), CAST(N'2018-08-15T16:00:00.000' AS DateTime), CAST(N'2018-08-14T17:15:00.000' AS DateTime), CAST(N'2018-08-14T17:00:00.000' AS DateTime), 7904.0000)
GO
INSERT [dbo].[PricePrediction] ([Id], [Name], [Description], [ResultPrice], [NumberOfPredictors], [Volume], [Coinbase], [UpdatedDate], [OpenBettingTime], [CloseBettingTime], [ResultTime], [ToBeComparedTime], [ToBeComparedPrice]) VALUES (6, N'Price Prediction #6', NULL, 8005.0000, 15, 15.0000, N'BTCUSDT', NULL, CAST(N'2018-08-14T01:16:00.000' AS DateTime), CAST(N'2018-08-16T00:00:00.000' AS DateTime), CAST(N'2018-08-15T01:15:00.000' AS DateTime), CAST(N'2018-08-15T01:00:00.000' AS DateTime), 7905.0000)
GO
SET IDENTITY_INSERT [dbo].[PricePrediction] OFF
GO
SET IDENTITY_INSERT [dbo].[PricePredictionHistory] ON 
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (2, 1, 1, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (3, 1, 2, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 800.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10800.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (4, 3, 3, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (5, 1, 4, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 250.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10250.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (6, 1, 5, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (7, 2, 6, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 1900.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 11900.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (8, 3, 7, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 3920.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 13920.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (9, 3, 8, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (10, 1, 9, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (11, 2, 10, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 7800.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 17800.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (12, 1, 11, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 12000.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 22000.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (13, 3, 12, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (14, 1, 13, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 100.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 11000.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (15, 2, 14, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (16, 2, 15, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 300.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10300.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (17, 1, 16, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 1, N'Lose', 0.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 0.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (18, 1, 17, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 500.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10500.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (19, 2, 12, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 500.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10500.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (20, 3, 19, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 500.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10500.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (21, 1, 20, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, N'Win', 500.0000, CAST(N'2018-08-13T09:15:00.000' AS DateTime), 10500.0000)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (22, 4, 12, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (23, 4, 12, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate], [TotalAward]) VALUES (24, 4, 12, CAST(N'2018-08-10T00:00:00.000' AS DateTime), 10000.0000, 0, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[PricePredictionHistory] OFF
GO
SET IDENTITY_INSERT [dbo].[SysUser] ON 
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (1, 1, N'info.cplcoin@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, 2, 1, NULL, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (2, 1, N'info.cplcoin2@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 39500.0000, NULL, NULL, NULL, NULL, NULL, 0, 2, 2, 1, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (3, 1, N'info.cplcoin3@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, 2, 3, 1, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (4, 1, N'info.cplcoin4@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 179700.0000, NULL, NULL, NULL, NULL, NULL, 0, 2, 4, 2, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (5, 1, N'info.cplcoin5@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 103000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 5, 2, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (6, 1, N'info.cplcoin6@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 69000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 6, 5, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (7, 1, N'info.cplcoin7@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 95900.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 7, NULL, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (8, 1, N'info.cplcoin8@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 8, 6, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (9, 1, N'info.cplcoin9@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 9, 7, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (10, 1, N'info.cplcoin10@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 10, 7, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (11, 1, N'info.cplcoin11@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, 2, 11, 2, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (12, 1, N'info.cplcoin12@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 12, 4, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (13, 1, N'info.cplcoin13@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 13, NULL, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (14, 1, N'info.cplcoin14@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 142900.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 14, NULL, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (15, 1, N'info.cplcoin15@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 145000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 15, 14, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (16, 1, N'info.cplcoin16@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 95000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 16, 15, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (17, 1, N'info.cplcoin17@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 57000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 17, 16, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (18, 1, N'info.cplcoin18@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 95000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 18, 17, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (19, 1, N'info.cplcoin19@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 19000.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 19, 18, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCAmount], [ETHAmount], [TokenAmount], [Gender], [PostalCode], [Country], [City], [StreetAddress], [TwoFactorAuthenticationEnable], [AgencyId], [AffiliateId], [IsIntroducedById], [AffiliateCreatedDate], [IsLocked]) VALUES (20, 1, N'info.cplcoin20@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2018-08-28T20:12:59.967' AS DateTime), NULL, NULL, NULL, NULL, 1, NULL, 0, N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', CAST(0.00000000 AS Decimal(18, 8)), CAST(0.00000000 AS Decimal(18, 8)), 0.0000, NULL, NULL, NULL, NULL, NULL, 0, NULL, 20, 19, CAST(N'2018-09-15T00:00:00.000' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[SysUser] OFF
GO
