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

GO

DECLARE @ConstraintName nvarchar(200)

SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('Lottery') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'DesktopTopImage' AND object_id = OBJECT_ID(N'Lottery'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE Lottery DROP CONSTRAINT ' + @ConstraintName)
IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('Lottery') AND name='DesktopTopImage')
EXEC('ALTER TABLE Lottery DROP COLUMN DesktopTopImage')

GO
DECLARE @ConstraintName nvarchar(200)

SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('Lottery') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'DesktopListingImage' AND object_id = OBJECT_ID(N'Lottery'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE Lottery DROP CONSTRAINT ' + @ConstraintName)
IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('Lottery') AND name='DesktopListingImage')
EXEC('ALTER TABLE Lottery DROP COLUMN DesktopListingImage')

GO
DECLARE @ConstraintName nvarchar(200)

SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('Lottery') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'MobileListingImage' AND object_id = OBJECT_ID(N'Lottery'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE Lottery DROP CONSTRAINT ' + @ConstraintName)
IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('Lottery') AND name='MobileListingImage')
EXEC('ALTER TABLE Lottery DROP COLUMN MobileListingImage')

GO
DECLARE @ConstraintName nvarchar(200)

SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('Lottery') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'MobileTopImage' AND object_id = OBJECT_ID(N'Lottery'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE Lottery DROP CONSTRAINT ' + @ConstraintName)
IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('Lottery') AND name='MobileTopImage')
EXEC('ALTER TABLE Lottery DROP COLUMN MobileTopImage')

GO
DECLARE @ConstraintName nvarchar(200)

SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS WHERE PARENT_OBJECT_ID = OBJECT_ID('Lottery') AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'PrizeImage' AND object_id = OBJECT_ID(N'Lottery'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE Lottery DROP CONSTRAINT ' + @ConstraintName)
IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('Lottery') AND name='PrizeImage')
EXEC('ALTER TABLE Lottery DROP COLUMN PrizeImage')

GO