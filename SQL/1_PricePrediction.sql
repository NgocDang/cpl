ALTER TABLE PricePrediction
	DROP COLUMN OpenTime, EndTime, PredictionResultTime, PredictionPrice;

ALTER TABLE PricePrediction
	ADD OpenBettingTime Datetime not null,
		CloseBettingTime Datetime not null,
		ResultTime Datetime not null,
		ToBeComparedTime Datetime not null,
		ToBeComparedPrice Money null;