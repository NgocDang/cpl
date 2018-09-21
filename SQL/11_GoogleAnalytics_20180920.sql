USE CPL;

INSERT INTO Setting VALUES (N'HomeViewId', '181557634', NULL);
INSERT INTO Setting VALUES (N'LotteryViewId', '181871809', NULL);
INSERT INTO Setting VALUES (N'PricePredictionViewId', '181838124', NULL);

GO

ALTER TABLE LotteryCategory ADD ViewId nvarchar(20) NOT NULL DEFAULT N'';

GO

UPDATE LotteryCategory SET ViewId = N'182104782' WHERE Id = 1;