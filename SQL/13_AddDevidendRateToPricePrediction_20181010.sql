ALTER TABLE PricePrediction
ADD DividendRate int NOT NULL DEFAULT 80;

insert into Setting values (N'PricePredictionTotalAwardPercentage', N'80', NULL);