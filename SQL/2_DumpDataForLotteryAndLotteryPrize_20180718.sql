USE [CPL]
GO

TRUNCATE TABLE [Lottery];

DBCC CHECKIDENT ('Lottery', RESEED, 1);

INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (1 , N'2018-05-29 00:00:00.000' , 2);
INSERT INTO [dbo].[Lottery]([Phase],[CreatedDate],[Volume]) VALUES (2 , N'2018-07-02 00:00:00.000' , 5000);

TRUNCATE TABLE [LotteryPrize];
DBCC CHECKIDENT ('LotteryPrize', RESEED, 1);

INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'1st' ,1000000 ,1 , 1, N'bg-warning');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'2nd' ,20000 ,1 , 5, N'bg-primary');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'3rd' ,1000 ,1 , 25, N'bg-danger');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'4th' ,500 ,1 , 500, N'bg-success');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'1st' ,1000000 ,2 , 1, N'bg-warning');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'2nd' ,20000 ,2 , 5, N'bg-primary');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'3rd' ,1000 ,2 , 25, N'bg-danger');
INSERT INTO [dbo].[LotteryPrize]([Name],[Value],[LotteryId],[Volume],[Color]) VALUES (N'4th' ,500 ,2 , 500, N'bg-success');

GO