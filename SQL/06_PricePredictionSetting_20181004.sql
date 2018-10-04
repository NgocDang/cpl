USE CPL;

ALTER TABLE PricePredictionSetting ADD Status int not null default 0;

GO

EXEC sp_RENAME 'PricePredictionSetting.DividedRate' , 'DividendRate', 'COLUMN';

GO
ALTER TABLE PricePredictionSetting ALTER COLUMN OpenBettingTime time not null;
ALTER TABLE PricePredictionSetting ALTER COLUMN CloseBettingTime time not null;