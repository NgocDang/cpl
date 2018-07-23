Use CPL;

ALTER TABLE Lottery ADD Status INT NOT NULL DEFAULT '1';

INSERT INTO Setting Values (N'LotteryGameRawingTimeInHour', N'12:00', N'Time to rawing lottery. From 0 to 24');

-- Update dump data
Update Lottery Set Status = 3 where Phase = 1;
Update Lottery Set Status = 2 where Phase = 2;
Update Lottery Set Volume = 5 where Phase = 2;