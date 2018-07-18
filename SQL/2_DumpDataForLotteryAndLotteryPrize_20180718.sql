USE [CPL]
GO

TRUNCATE TABLE [Lottery];

DBCC CHECKIDENT ('Lottery', RESEED, 1);

INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (1 , N'2018-05-29 00:00:00.000' , 2);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (2 , N'2018-07-02 00:00:00.000' , 5000);

TRUNCATE TABLE [LotteryPrize];
DBCC CHECKIDENT ('LotteryPrize', RESEED, 1);

INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,1 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,1 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,1 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,1 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,2 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,2 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,2 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,2 ,500);

GO


