INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'DeleteSuccessfully', N'Delete Successfully')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'DeleteSuccessfully', N'削除が正常に終了しました')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'UpdateSuccessfully', N'Update Successfully')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'UpdateSuccessfully', N'更新が正常に完了しました')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'IsEnabled', N'is enabled')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'IsEnabled', N'有効になっています')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'IsNotEnabled', N'is not enabled.')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'IsNotEnabled', N'無効')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'SuccessfullyVerified', N'Successfully verified.')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'SuccessfullyVerified', N'正常に確認されました。')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'KYCIsPending', N'KYC Verification is pending')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'KYCIsPending', N'KYC確認が保留中')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'KYCNotVerified', N'Your KYC is not verified.')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'KYCNotVerified', N'未提出')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'ETHHDWalletAddress', N'ETH Receive Address')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'ETHHDWalletAddress', N'ETH送付先アドレス')

INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'BTCHDWalletAddress', N'BTC Receive Address')
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (2, N'BTCHDWalletAddress', N'BTC送付先アドレス')

UPDATE LangDetail SET Value = N'Game History' WHERE name = N'GameHistory' and LangId = 1