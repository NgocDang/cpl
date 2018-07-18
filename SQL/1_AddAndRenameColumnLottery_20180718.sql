Use CPL;

ALTER TABLE Lottery ADD Volume INT NOT NULL DEFAULT '0';

ALTER TABLE LotteryPrize ADD Volume INT NOT NULL DEFAULT '0';
ALTER TABLE LotteryPrize ADD Color NVARCHAR(15) NOT NULL DEFAULT '';

EXEC sp_rename 'LotteryPrize.Amount', 'Value', 'COLUMN';