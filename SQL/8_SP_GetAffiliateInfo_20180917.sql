GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAffiliateInfo]
	-- Add the parameters for the stored procedure here
	--@SysUserId int,
	--@PeriodInDay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here

--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--/////////////////////////////////////////////// SALE & INTRODUCED USERS ///////////////////////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--

	SELECT 
		-------------------------------
		-- TotalSale.Tier1DirectSale --
		-------------------------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
					FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
					WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
							and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
							and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
					    and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE   PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
						--)) Tier1Direct(Value))
			+
		------------------------------------
		-- TotalSale.Tier2SaleToTier1Sale --
		------------------------------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier2IntroducedUsers, ','))),0)
					--)) Tier2ToTier1(Value))
			+
		------------------------------------
		-- TotalSale.Tier3SaleToTier1Sale --
		------------------------------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE	PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE	PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate()) 
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.Tier3IntroducedUsers, ','))),0)
					--)) Tier3ToTier1(Value))
		AS TotalSale,

		----------------
		-- DirectSale --
		----------------
		--(SELECT MAX(Value) FROM (VALUES (0), (
			ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
					FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
					WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
							and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
							and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
				+
			ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
					    and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
			-
			ISNULL((SELECT SUM(Award) as TotalDirectCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE   PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(iu.DirectIntroducedUsers, ','))),0)
						--)) Tier1Direct(Value))
		AS DirectSale,

		--------------------------
		-- TotalIntroducedUsers --
		--------------------------
			ISNULL((LEN(DirectIntroducedUsers) - LEN(REPLACE(DirectIntroducedUsers,',','')) + 1),0)
			+ 
			ISNULL((LEN(Tier2IntroducedUsers) - LEN(REPLACE(Tier2IntroducedUsers,',','')) + 1),0)
			+ 
			ISNULL((LEN(Tier3IntroducedUsers) - LEN(REPLACE(Tier3IntroducedUsers,',','')) + 1),0)
		AS TotalIntroducedUsers,

		---------------------------
		-- DirectIntroducedUsers --
		---------------------------
			ISNULL((LEN(DirectIntroducedUsers) - LEN(REPLACE(DirectIntroducedUsers,',','')) + 1),0)
		AS DirectIntroducedUsers
	FROM SysUser su join IntroducedUsers iu on su.Id = iu.Id
	WHERE su.Id = @SysUserId and su.AffiliateId is not null and su.AffiliateId > 0







--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--///////////////////////////////////// INTRODUCED TIER 2 & TIER 3 USERS IN DETAILS /////////////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
	DECLARE @SysUserId int;
	SET @SysUserId = 1;

	DECLARE @PeriodInDay int;
	SET @PeriodInDay = 300;

	DECLARE @DirectIntroducedUsers nvarchar(MAX);
	DECLARE @Tier2IntroducedUsers nvarchar(MAX);
	SELECT  @DirectIntroducedUsers = DirectIntroducedUsers, 
			@Tier2IntroducedUsers = Tier2IntroducedUsers 
	FROM	IntroducedUsers
	WHERE	Id = @SysUserId;

	SELECT 
		--------
		-- Id --
		--------
		su.Id,

		----------------
		-- KindOfTier --
		----------------
		CASE WHEN su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
			 THEN 2 -- Tier 2
			 WHEN su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))
			 THEN 3 -- Tier 3
			 END
		AS KindOfTier,

		-------------
		-- UsedCPL --
		-------------
		ISNULL((SELECT SUM(UnitPrice)
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId = su.Id),0)
		+
		ISNULL((SELECT SUM(Amount) 
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
					    and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId = su.Id),0)
		As UsedCPL,
								 
		-------------
		-- LostCPL --
		-------------
		ISNULL((SELECT SUM(UnitPrice)
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId = su.Id),0)
		-
		ISNULL((SELECT SUM(LotteryPrize.Value)
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId = LotteryPrize.Id
				WHERE LotteryHistory.Result = 'WIN' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId = su.Id),0)
		+
		ISNULL((SELECT SUM(Amount) 
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
					    and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId = su.Id),0)
		-
		ISNULL((SELECT SUM(TotalAward) 
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result = 'WIN' -- WIN / LOSE 
					    and PricePrediction.ResultTime >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId = su.Id),0)
		As LostCPL,

		-------------------
		-- AffiliateSale --
		-------------------
		    ---------------------------
			-- Direct affiliate sale --
			---------------------------
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and Lottery.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			---------------------------
			-- Tier 2 affiliate sale --
			---------------------------
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			---------------------------
			-- Tier 3 affiliate sale --
			---------------------------
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
									join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
		--				)) Total(Value))
		As AffiliateSale,


	FROM   SysUser su join IntroducedUsers iu on su.Id = iu.Id
	WHERE (su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
		or su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ',')))
	   and su.AffiliateId is not null 
	   and su.AffiliateId > 0
END
