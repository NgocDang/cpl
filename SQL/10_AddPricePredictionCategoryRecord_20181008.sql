USE CPL;

GO
SET IDENTITY_INSERT [dbo].[PricePredictionCategory] ON 
GO
INSERT [dbo].[PricePredictionCategory] ([Id]) VALUES (1)
GO
INSERT [dbo].[PricePredictionCategory] ([Id]) VALUES (2)
GO
SET IDENTITY_INSERT [dbo].[PricePredictionCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[PricePredictionCategoryDetail] ON 
GO
INSERT [dbo].[PricePredictionCategoryDetail] ([Id], [Name], [Description], [PricePredictionCategoryId], [LangId]) VALUES (1, N'Standard', NULL, 1, 1)
GO
INSERT [dbo].[PricePredictionCategoryDetail] ([Id], [Name], [Description], [PricePredictionCategoryId], [LangId]) VALUES (2, N'スタンダード', NULL, 1, 2)
GO
INSERT [dbo].[PricePredictionCategoryDetail] ([Id], [Name], [Description], [PricePredictionCategoryId], [LangId]) VALUES (3, N'Variety', NULL, 2, 1)
GO
INSERT [dbo].[PricePredictionCategoryDetail] ([Id], [Name], [Description], [PricePredictionCategoryId], [LangId]) VALUES (4, N'バラエティー', NULL, 2, 2)
GO
SET IDENTITY_INSERT [dbo].[PricePredictionCategoryDetail] OFF
