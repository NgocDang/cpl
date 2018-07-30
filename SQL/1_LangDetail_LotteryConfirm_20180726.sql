INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'Phase', N'Phase')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'CreatedDate', N'Created Date')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'LotteryTicketNumber', N'Ticket Number')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'UpdatedDate', N'Updated Date')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'PurchaseFailed', N'Purchase failed, please try again!')

ALTER TABLE LotteryHistory ADD TxHash nvarchar(100) NOT NULL DEFAULT ''
ALTER TABLE LotteryHistory ALTER COLUMN TicketNumber nvarchar(6) NULL