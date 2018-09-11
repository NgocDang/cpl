Use CPL;

INSERT INTO LotteryDetail (LotteryId, LangId, DesktopTopImage, DesktopListingImage, MobileListingImage, MobileTopImage, PrizeImage, Description)
SELECT Id, 1, DesktopTopImage, DesktopListingImage, MobileListingImage, MobileTopImage, PrizeImage, N''
FROM Lottery;

GO

INSERT INTO LotteryDetail (LotteryId, LangId, DesktopTopImage, DesktopListingImage, MobileListingImage, MobileTopImage, PrizeImage, Description)
SELECT Id, 2, DesktopTopImage, DesktopListingImage, MobileListingImage, MobileTopImage, PrizeImage, N''
FROM Lottery;

GO

UPDATE LotteryDetail
SET Description = (SELECT TOP 1 Value FROM LangMsgDetail WHERE Name = 'LotteryDescription' and LangId = 1)
WHERE LangID = 1;

GO

UPDATE LotteryDetail
SET Description = (SELECT TOP 1 Value FROM LangMsgDetail WHERE Name = 'LotteryDescription' and LangId = 2)
WHERE LangID = 2;