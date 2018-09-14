USE [CPL]
GO
/****** Object:  StoredProcedure [dbo].[usp_CreatePayment]    Script Date: 13/09/2018 4:58:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_CreatePayment]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- List of all Tier 1 Agencies
	DECLARE @allTier1Agencies TABLE (Id int);
	INSERT INTO @allTier1Agencies 
	SELECT Id 
	FROM SysUser 
	WHERE AffiliateId is not null and AffiliateId > 0 and AgencyId is not null and IsIntroducedById is null;

	-- List of all Tier 2 Agencies
	DECLARE @allTier2Agencies TABLE (Id int);
	INSERT INTO @allTier2Agencies 
	SELECT Id 
	FROM SysUser 
	WHERE AffiliateId is not null and AffiliateId > 0 and AgencyId is not null and IsIntroducedById in (select Id from @allTier1Agencies);

	-- List of all Tier 3 Agencies
	DECLARE @allTier3Agencies TABLE (Id int);
	INSERT INTO @allTier3Agencies 
	SELECT Id 
	FROM SysUser 
	WHERE AffiliateId is not null and AffiliateId > 0 and AgencyId is not null and IsIntroducedById in (select Id from @allTier2Agencies);

	--SELECT * FROM @allTier1Agencies;
	--SELECT * FROM @allTier2Agencies;
	--SELECT * FROM @allTier3Agencies;

	-- List of user and introduced users
	DECLARE @introducedUsers TABLE (Id int, DirectIntroducedUsers nvarchar(MAX), Tier2IntroducedUsers nvarchar(MAX), Tier3IntroducedUsers nvarchar(MAX));
	INSERT INTO @introducedUsers 
	SELECT 
		Id, 
		-- Direct introduced users
		(SELECT STRING_AGG(Id, ',') AS DirectIntroducedUsers
		FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id),

		-- Tier 2 introduced users
		(SELECT STRING_AGG(Id, ',') AS Tier2IntroducedUsers
		FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
			(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)),
		-- Tier 3 introduced users
		(SELECT STRING_AGG(Id, ',') AS Tier3IntroducedUsers
		FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
					(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
						(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)))
	FROM SysUser su
	WHERE AffiliateId is not null and AffiliateId > 0;
	--SELECT * FROM @introducedUsers;

	DECLARE @Tier1DirectSale money;
	INSERT INTO Payment
	SELECT 
		-----------------------------------------------
		-- SysUserId to be inserted to PAYMENT table --
		-----------------------------------------------
		su.Id, 

		-------------------------------------------------
		-- CreatedDate to be inserted to PAYMENT table --
		-------------------------------------------------
		getdate() AS CreatedDate, 

		-------------------------------------------------
		-- UpdatedDate to be inserted to PAYMENT table --
		-------------------------------------------------
		null AS UpdatedDate, 

		-----------------------------------------------------
		-- Tier1DirectSale to be inserted to PAYMENT table --
		-----------------------------------------------------
		(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE Lottery.Status = 3 -- COMPLETED
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
				-
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE Lottery.Status = 3 -- COMPLETED
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate()))
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE   DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate()))
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
				-
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate()))
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and  PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
						)) Tier1Direct(Value))
		AS Tier1DirectSale,

		----------------------------------------------------------
		-- Tier2SaleToTier1Sale to be inserted to PAYMENT table --
		----------------------------------------------------------
		(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE Lottery.Status = 3 -- COMPLETED
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
				-
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE Lottery.Status = 3 -- COMPLETED
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
				-
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and  PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
					)) Tier2ToTier1(Value))
		AS Tier2SaleToTier1Sale,

		----------------------------------------------------------
		-- Tier3SaleToTier1Sale to be inserted to PAYMENT table --
		----------------------------------------------------------
		(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE Lottery.Status = 3 -- COMPLETED
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
				-
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE Lottery.Status = 3 -- COMPLETED
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE	DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
				-
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE	DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and  PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
					)) Tier3ToTier1(Value))
		AS Tier3SaleToTier1Sale,

		-----------------------------------------------------
		-- Tier1DirectRate to be inserted to PAYMENT table --
		-----------------------------------------------------
		CASE	WHEN EXISTS (SELECT 1 FROM @allTier1Agencies allTier1Agencies WHERE su.Id = allTier1Agencies.Id) 
				THEN 
					(SELECT Tier1DirectRate FROM Agency WHERE Id = su.AgencyId)
				WHEN EXISTS (SELECT 1 FROM @allTier2Agencies allTier2Agencies WHERE su.Id = allTier2Agencies.Id) 
				THEN
					(SELECT Tier2DirectRate FROM Agency WHERE Id = su.AgencyId)
				WHEN EXISTS (SELECT 1 FROM @allTier3Agencies allTier3Agencies WHERE su.Id = allTier3Agencies.Id) 
				THEN
					(SELECT Tier3DirectRate FROM Agency WHERE Id = su.AgencyId)
				ELSE
					(SELECT Tier1DirectRate FROM Affiliate WHERE Id = su.AffiliateId)
		END		
				AS Tier1DirectRate, 

		----------------------------------------------------------
		-- Tier2SaleToTier1Rate to be inserted to PAYMENT table --
		----------------------------------------------------------
		CASE	WHEN EXISTS (SELECT 1 FROM @allTier1Agencies allTier1Agencies WHERE su.Id = allTier1Agencies.Id) 
				THEN 
					(SELECT Tier2SaleToTier1Rate FROM Agency WHERE Id = su.AgencyId)
				WHEN EXISTS (SELECT 1 FROM @allTier2Agencies allTier2Agencies WHERE su.Id = allTier2Agencies.Id) 
				THEN
					(SELECT Tier3SaleToTier2Rate FROM Agency WHERE Id = su.AgencyId)
				ELSE
					(SELECT Tier2SaleToTier1Rate FROM Affiliate WHERE Id = su.AffiliateId)
		END		
				AS Tier2SaleToTier1Rate,

		----------------------------------------------------------
		-- Tier3SaleToTier1Rate to be inserted to PAYMENT table --
		----------------------------------------------------------
		CASE	WHEN EXISTS (SELECT 1 FROM @allTier1Agencies allTier1Agencies WHERE su.Id = allTier1Agencies.Id)
				THEN 
					(SELECT Tier3SaleToTier1Rate FROM Agency WHERE Id = su.AgencyId)
				ELSE
					(SELECT Tier3SaleToTier1Rate FROM Affiliate WHERE Id = su.AffiliateId)
		END		
				AS Tier3SaleToTier1Rate
				
	FROM SysUser su join @introducedUsers iu on su.Id = iu.Id
	WHERE su.AffiliateId is not null and su.AffiliateId > 0
END
