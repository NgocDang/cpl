USE [CPL]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetAffiliateSale]    Script Date: 9/14/2018 4:06:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAffiliateSale]
	-- Add the parameters for the stored procedure here
	@UserId int,
	@TotalSale money OUTPUT,
	@TodaySale money OUTPUT,
	@YesterdaySale money OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- List of introduced users by @UserId
	DECLARE @directIntroducedUsers nvarchar(MAX);
	DECLARE @tier2IntroducedUsers nvarchar(MAX);
	DECLARE @tier3IntroducedUsers nvarchar(MAX);
	SELECT 
		-- Direct introduced users
		@directIntroducedUsers = DirectIntroducedUsers,
		-- Tier 2 introduced users
		@tier2IntroducedUsers = Tier2IntroducedUsers,
		-- Tier 3 introduced users
		@tier3IntroducedUsers = Tier3IntroducedUsers

	FROM dbo.IntroducedUsers
	WHERE Id = @UserId;

	SELECT @directIntroducedUsers as Direct, @tier2IntroducedUsers as Tier2, @tier3IntroducedUsers as tier3

    -- Insert statements for procedure here
	SELECT @TotalSale = TotalSale,
		@TodaySale = TodaySale,
		@YesterdaySale = YesterdaySale
	FROM (
	SELECT
		----------------
		-- Total sale --
		----------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			-----------------
			-- Direct sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-- Plus tier 1 sale
				+
			-----------------
			-- Tier 2 sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			----- Price prediction
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-- Plus tier 1 sale
				+
			-----------------
			-- Tier 3 sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			----- Price prediction
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
		--				)) Total(Value))
		As TotalSale,

		----------------
		-- Today sale --
		----------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			-----------------
			-- Direct sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE
						and Lottery.UpdatedDate >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and Lottery.UpdatedDate < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE
						and LotteryHistory.UpdatedDate >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.UpdatedDate < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-- Plus tier 1 sale
				+
			-----------------
			-- Tier 2 sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and Lottery.UpdatedDate < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.UpdatedDate < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			----- Price prediction
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-- Plus tier 1 sale
				+
			-----------------
			-- Tier 3 sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and Lottery.UpdatedDate < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.UpdatedDate < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			----- Price prediction
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,-1,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			--			)) Today(Value))
		As TodaySale,

		--------------------
		-- Yesterday sale --
		--------------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			-----------------
			-- Direct sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE
						and	Lottery.UpdatedDate >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and Lottery.UpdatedDate < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE
						and	LotteryHistory.UpdatedDate >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and LotteryHistory.UpdatedDate < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@directIntroducedUsers, ','))),0)
			-- Plus tier 1 sale
				+
			-----------------
			-- Tier 2 sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and	Lottery.UpdatedDate >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and Lottery.UpdatedDate < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and	LotteryHistory.UpdatedDate >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and LotteryHistory.UpdatedDate < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			----- Price prediction
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier2IntroducedUsers, ','))),0)
			-- Plus tier 1 sale
				+
			-----------------
			-- Tier 3 sale --
			-----------------
			----- Lottery
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and	Lottery.UpdatedDate >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and Lottery.UpdatedDate < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and	LotteryHistory.UpdatedDate >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and LotteryHistory.UpdatedDate < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-- Plus Price prediction
				+
			----- Price prediction
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePrediction.ResultTime >= DATEADD(day,DATEDIFF(day,1,GETDATE()),0)
						and PricePrediction.ResultTime < DATEADD(day,DATEDIFF(day,0,GETDATE()),0)
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@tier3IntroducedUsers, ','))),0)
		--				)) Yesterday(Value))
		As YesterdaySale
	FROM SysUser su
	WHERE Id = @UserId and AffiliateId is not null and AffiliateId > 0
	) AffiliateSale

	RETURN
END
