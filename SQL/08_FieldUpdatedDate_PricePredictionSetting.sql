use CPL;

ALTER TABLE PricePredictionSetting
ADD UpdatedDate datetime NULL;

GO

ALTER TABLE PricePredictionSetting
ADD IsDeleted bit not NULL;