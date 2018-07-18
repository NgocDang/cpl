USE [CPL]
GO

TRUNCATE TABLE [Lottery] GO;

DBCC CHECKIDENT ('Lottery', RESEED, 1) GO;

INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (1 , N'2017-05-02 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (2 , N'2017-06-30 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (3 , N'2017-07-02 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (4 , N'2017-08-03 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (5 , N'2017-08-12 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (6 , N'2017-09-01 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (7 , N'2017-09-21 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (8 , N'2017-10-08 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (9 , N'2017-11-05 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (10 , N'2017-12-12 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (11 , N'2018-01-12 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (12 , N'2018-02-22 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (13 , N'2018-03-09 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (14 , N'2018-04-02 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (15 , N'2018-05-11 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (16 , N'2018-06-12 00:00:00.000' , 5000);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (17 , N'2018-07-01 00:00:00.000' , 5000);

GO;

TRUNCATE TABLE [LotteryPrize] GO;
DBCC CHECKIDENT ('LotteryPrize', RESEED, 1) GO;

INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,1 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,1 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,1 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,1 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,2 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,2 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,2 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,2 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,3 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,3 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,3 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,3 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,4 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,4 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,4 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,4 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,5 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,5 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,5 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,5 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,6 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,6 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,6 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,6 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,7 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,7 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,7 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,7 ,500);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000000 ,8 ,1);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,20000 ,8 ,5);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,1000 ,8 ,25);
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume]) VALUES (NULL ,500 ,8 ,500);

GO


