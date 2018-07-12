USE [CPL]
GO
SET IDENTITY_INSERT [dbo].[LotteryHistory] ON 

INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (1, 1, 20, N'LOSE', CAST(N'2018-07-02T00:00:00.000' AS DateTime), N'123456', CAST(N'2018-07-03T00:00:00.000' AS DateTime), NULL, 1)
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (2, 1, 20, N'LOSE', CAST(N'2018-07-03T00:00:00.000' AS DateTime), N'123654', CAST(N'2018-07-03T00:00:00.000' AS DateTime), NULL, 2)
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (3, 2, 20, N'WIN', CAST(N'2018-07-05T00:00:00.000' AS DateTime), N'132523', CAST(N'2018-07-06T00:00:00.000' AS DateTime), 4, 30)
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (4, 2, 20, N'LOSE', CAST(N'2018-07-05T00:00:00.000' AS DateTime), N'123123', CAST(N'2018-07-06T00:00:00.000' AS DateTime), NULL, 31)
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (5, 2, 20, N'LOSE', CAST(N'2018-07-05T00:00:00.000' AS DateTime), N'123258', CAST(N'2018-07-06T00:00:00.000' AS DateTime), NULL, 32)
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (6, 2, 20, N'WIN', CAST(N'2018-07-05T00:00:00.000' AS DateTime), N'123301', CAST(N'2018-07-06T00:00:00.000' AS DateTime), 6, 33)
INSERT [dbo].[LotteryHistory] ([Id], [LotteryId], [SysUserId], [Result], [CreatedDate], [TicketNumber], [UpdatedDate], [LotteryPrizeId], [TicketIndex]) VALUES (7, 2, 20, N'LOSE', CAST(N'2018-07-05T00:00:00.000' AS DateTime), N'215351', CAST(N'2018-07-06T00:00:00.000' AS DateTime), NULL, 34)
SET IDENTITY_INSERT [dbo].[LotteryHistory] OFF
SET IDENTITY_INSERT [dbo].[LotteryPrize] ON 

INSERT [dbo].[LotteryPrize] ([Id], [Name], [Amount], [LotteryId]) VALUES (1, NULL, 1000000.0000, 1)
INSERT [dbo].[LotteryPrize] ([Id], [Name], [Amount], [LotteryId]) VALUES (2, NULL, 50000.0000, 1)
INSERT [dbo].[LotteryPrize] ([Id], [Name], [Amount], [LotteryId]) VALUES (3, NULL, 1000.0000, 1)
INSERT [dbo].[LotteryPrize] ([Id], [Name], [Amount], [LotteryId]) VALUES (4, NULL, 2000000.0000, 2)
INSERT [dbo].[LotteryPrize] ([Id], [Name], [Amount], [LotteryId]) VALUES (5, NULL, 200000.0000, 2)
INSERT [dbo].[LotteryPrize] ([Id], [Name], [Amount], [LotteryId]) VALUES (6, NULL, 0.0000, 2)
SET IDENTITY_INSERT [dbo].[LotteryPrize] OFF
SET IDENTITY_INSERT [dbo].[PricePredictionHistory] ON 

INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate]) VALUES (1, 1, 20, CAST(N'2018-06-30T13:00:00.000' AS DateTime), 500.0000, 1, N'WIN', 1500.0000, CAST(N'2018-07-06T00:00:00.000' AS DateTime))
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate]) VALUES (2, 2, 20, CAST(N'2018-07-01T00:00:00.000' AS DateTime), 100.0000, 0, N'LOSE', 0.0000, CAST(N'2018-07-07T00:00:00.000' AS DateTime))
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate]) VALUES (3, 3, 20, CAST(N'2018-07-07T01:00:00.000' AS DateTime), 1000.0000, 0, N'KYC_PENDING', 2000.0000, CAST(N'2018-07-08T00:00:00.000' AS DateTime))
INSERT [dbo].[PricePredictionHistory] ([Id], [PricePredictionId], [SysUserId], [CreatedDate], [Amount], [Prediction], [Result], [Award], [UpdatedDate]) VALUES (4, 4, 20, CAST(N'2018-07-08T00:00:00.000' AS DateTime), 2000.0000, 1, N'LOSE', 0.0000, CAST(N'2018-07-09T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[PricePredictionHistory] OFF
