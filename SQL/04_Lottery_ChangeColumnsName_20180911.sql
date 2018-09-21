USE CPL;
GO

SP_RENAME 'Lottery.DesktopSlideImage', 'DesktopTopImage', 'COLUMN';
GO
SP_RENAME 'Lottery.MobileSlideImage', 'MobileTopImage', 'COLUMN';
GO

UPDATE LangDetail SET Name = N'DesktopTopImage' WHERE Name = N'DesktopSlideImage';
UPDATE LangDetail SET Name = N'MobileTopImage' WHERE Name = N'MobileSlideImage';

UPDATE LangDetail SET Name = N'DesktopTopImageRequired' WHERE Name = N'DesktopSlideImageRequired';
UPDATE LangDetail SET Name = N'MobileTopImageRequired' WHERE Name = N'MobileSlideImageRequired';

Go