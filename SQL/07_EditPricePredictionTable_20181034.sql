USE CPL;
ALTER TABLE PricePrediction
	DROP  column Name, Description;

ALTER TABLE PricePredictionSetting
	ADD UpdatedDate datetime NULL;