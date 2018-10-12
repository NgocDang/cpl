USE [CPL]
GO
DECLARE @ConstraintName nvarchar(200)

SELECT @ConstraintName = Name FROM SYS.DEFAULT_CONSTRAINTS 
WHERE PARENT_OBJECT_ID = OBJECT_ID('PricePrediction') 
	AND PARENT_COLUMN_ID = (SELECT column_id FROM sys.columns WHERE NAME = N'IsCreatedByAdmin' AND object_id = OBJECT_ID(N'PricePrediction'))
IF @ConstraintName IS NOT NULL
EXEC('ALTER TABLE PricePrediction DROP CONSTRAINT ' + @ConstraintName)
IF EXISTS (SELECT * FROM syscolumns WHERE id=object_id('PricePrediction') AND name='IsCreatedByAdmin')
EXEC('ALTER TABLE PricePrediction DROP COLUMN IsCreatedByAdmin')
GO