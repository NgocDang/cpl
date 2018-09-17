USE [CPL]
GO
/****** Object:  StoredProcedure [dbo].[usp_CreatePayment]    Script Date: 17/09/2018 9:42:31 AM ******/
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
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
					FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
					WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
							and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate()))
							and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
							and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
					    and DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate()))
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and  PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE   PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate()))
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
						--)) Tier1Direct(Value))
		AS Tier1DirectSale,

		----------------------------------------------------------
		-- Tier2SaleToTier1Sale to be inserted to PAYMENT table --
		----------------------------------------------------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and  PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
					--)) Tier2ToTier1(Value))
		AS Tier2SaleToTier1Sale,

		----------------------------------------------------------
		-- Tier3SaleToTier1Sale to be inserted to PAYMENT table --
		----------------------------------------------------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, Lottery.UpdatedDate) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, Lottery.UpdatedDate) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE	PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and  PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE	PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and DATEPART(yyyy, PricePrediction.ResultTime) = DATEPART(yyyy, DATEADD(m, -1, getdate())) 
						and DATEPART(m, PricePrediction.ResultTime) = DATEPART(m, DATEADD(m, -1, getdate())) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
					--)) Tier3ToTier1(Value))
		AS Tier3SaleToTier1Sale,

		-----------------------------------------------------
		-- Tier1DirectRate to be inserted to PAYMENT table --
		-----------------------------------------------------
		CASE	WHEN EXISTS (SELECT 1 FROM Tier1Agencies WHERE su.Id = Tier1Agencies.Id) 
				THEN 
					(SELECT Tier1DirectRate FROM Agency WHERE Id = su.AgencyId)
				WHEN EXISTS (SELECT 1 FROM Tier2Agencies WHERE su.Id = Tier2Agencies.Id) 
				THEN
					(SELECT Tier2DirectRate FROM Agency WHERE Id = su.AgencyId)
				WHEN EXISTS (SELECT 1 FROM Tier3Agencies WHERE su.Id = Tier3Agencies.Id) 
				THEN
					(SELECT Tier3DirectRate FROM Agency WHERE Id = su.AgencyId)
				ELSE
					(SELECT Tier1DirectRate FROM Affiliate WHERE Id = su.AffiliateId)
		END		
				AS Tier1DirectRate, 

		----------------------------------------------------------
		-- Tier2SaleToTier1Rate to be inserted to PAYMENT table --
		----------------------------------------------------------
		CASE	WHEN EXISTS (SELECT 1 FROM Tier1Agencies WHERE su.Id = Tier1Agencies.Id) 
				THEN 
					(SELECT Tier2SaleToTier1Rate FROM Agency WHERE Id = su.AgencyId)
				WHEN EXISTS (SELECT 1 FROM Tier2Agencies WHERE su.Id = Tier2Agencies.Id) 
				THEN
					(SELECT Tier3SaleToTier2Rate FROM Agency WHERE Id = su.AgencyId)
				ELSE
					(SELECT Tier2SaleToTier1Rate FROM Affiliate WHERE Id = su.AffiliateId)
		END		
				AS Tier2SaleToTier1Rate,

		----------------------------------------------------------
		-- Tier3SaleToTier1Rate to be inserted to PAYMENT table --
		----------------------------------------------------------
		CASE	WHEN EXISTS (SELECT 1 FROM Tier1Agencies WHERE su.Id = Tier1Agencies.Id)
				THEN 
					(SELECT Tier3SaleToTier1Rate FROM Agency WHERE Id = su.AgencyId)
				ELSE
					(SELECT Tier3SaleToTier1Rate FROM Affiliate WHERE Id = su.AffiliateId)
		END		
				AS Tier3SaleToTier1Rate
				
	FROM SysUser su join IntroducedUsers iu on su.Id = iu.Id
	WHERE su.AffiliateId is not null and su.AffiliateId > 0
END
