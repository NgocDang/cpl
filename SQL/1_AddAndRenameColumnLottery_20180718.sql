Use CPL;

ALTER TABLE Lottery ADD Volume INT NOT NULL DEFAULT '0';

ALTER TABLE LotteryPrize ADD Volume INT NOT NULL DEFAULT '0';

EXEC sp_rename 'LotteryPrize.Amount', 'Value', 'COLUMN';