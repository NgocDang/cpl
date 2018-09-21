USE CPL;

INSERT INTO LotteryCategory VALUES (N'Lottery 1', NULL);

ALTER TABLE Lottery
ADD LotteryCategoryId int NOT NULL DEFAULT 1;