use CPL;

ALTER TABLE PricePredictionSetting
ADD UpdatedDate datetime NULL;

ALTER TABLE PricePredictionSetting
ADD IsDeleted bit not NULL;