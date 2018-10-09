USE [CPL];

ALTER TABLE PricePrediction
	ADD IsCreatedByAdmin BIT NOT NULL DEFAULT 0;

INSERT INTO Setting VALUES (N'AdminPricePredictionDailyJobStartHour', N'0', NULL);
INSERT INTO Setting VALUES (N'AdminPricePredictionDailyJobStartMinute', N'0', NULL);