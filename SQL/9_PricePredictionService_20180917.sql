USE CPL

INSERT INTO Setting VALUES (N'PricePredictionGameIntervalInHour', 24, NULL);
DELETE Setting WHERE [Name] = N'PricePredictionDailyStartTimeInHour';
DELETE Setting WHERE [Name] = N'PricePredictionDailyStartTimeInMinute';