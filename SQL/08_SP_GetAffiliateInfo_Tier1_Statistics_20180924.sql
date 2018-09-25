USE [CPL]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAffiliateInfo_Tier1_Statistics]
	-- Add the parameters for the stored procedure here
	@SysUserId int,
	@PeriodInDay int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here

DECLARE @DirectIntroducedUsers nvarchar(MAX)
DECLARE @Tier2IntroducedUsers nvarchar(MAX)
DECLARE @Tier3IntroducedUsers nvarchar(MAX)
DECLARE @DirectIntroducedUsersOnPeriod nvarchar(MAX)
DECLARE @Tier2IntroducedUsersOnPeriod nvarchar(MAX)
DECLARE @Tier3IntroducedUsersOnPeriod nvarchar(MAX)

-- INTRODUCED USERS REGARDLESS PERIOD
SELECT	@DirectIntroducedUsers = IntroducedUsers.DirectIntroducedUsers,
		@Tier2IntroducedUsers = IntroducedUsers.Tier2IntroducedUsers,
		@Tier3IntroducedUsers = IntroducedUsers.Tier3IntroducedUsers
FROM IntroducedUsers 
WHERE IntroducedUsers.Id = @SysUserId;

-- INTRODUCED USER ON PERIOD
SELECT @DirectIntroducedUsersOnPeriod = STRING_AGG(Id, ',')
FROM SysUser Tier1
WHERE Tier1.IsIntroducedById = @SysUserId and Tier1.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())

--Tier 2 Introduced Users on period
SELECT @Tier2IntroducedUsersOnPeriod = STRING_AGG(Id, ',') 
FROM SysUser Tier2 
WHERE	  Tier2.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	  and Tier2.IsIntroducedById in 
			(SELECT	Id 
			 FROM	SysUser Tier1
			 WHERE	Tier1.IsIntroducedById = @SysUserId and Tier1.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate()))

--Tier 3 Introduced Users on period
SELECT @Tier3IntroducedUsersOnPeriod = STRING_AGG(Id, ',') 
FROM SysUser Tier3 
WHERE	  Tier3.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	  and Tier3.IsIntroducedById in 
		   (SELECT	Id 
			FROM	SysUser Tier2
			WHERE	Tier2.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and Tier2.IsIntroducedById in 
					   (SELECT	Id
						FROM	SysUser Tier1 
						WHERE	Tier1.IsIntroducedById = @SysUserId and Tier1.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())))


--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--/////////////////////////////////// DATATABLE #1 -  SALE & INTRODUCED USERS ///////////////////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--

SELECT 
	--------------------------------------------------------
	-- Total Affiliate Sale | Tier 1 Direct Afiliate Sale --
	--------------------------------------------------------
	ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
			FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
					and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
		FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
		WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
				and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	+
	ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
		FROM PricePredictionHistory 
		WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(TotalAward) as TotalDirectCPLAwardedInPricePrediction
		FROM PricePredictionHistory 
		WHERE   PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	+
	--------------------------------------------------------------------------
	-- Total Affiliate Sale | Tier 2 Affiliate Sale to Tier 1 Afiliate Sale --
	--------------------------------------------------------------------------
	ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
		FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
		WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
				and LotteryHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
		FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
		WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
				and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))),0)
	+
	ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
		FROM PricePredictionHistory 
		WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(TotalAward) as TotalDirectCPLAwardedInPricePrediction
		FROM PricePredictionHistory 
		WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))),0)
	+
	--------------------------------------------------------------------------
	-- Total Affiliate Sale | Tier 3 Affiliate Sale to Tier 1 Afiliate Sale --
	--------------------------------------------------------------------------
	ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
		FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
		WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
				and LotteryHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
		FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
		WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
				and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))),0)
	+
	ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
		FROM PricePredictionHistory 
		WHERE	PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(TotalAward) as TotalDirectCPLAwardedInPricePrediction
		FROM PricePredictionHistory 
		WHERE	PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate()) 
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))),0)
AS TotalAffiliateSale,

---------------------------
-- Direct Affiliate Sale --
---------------------------
	ISNULL((SELECT SUM(UnitPrice) as TotalDirectCPLUsedInLottery
			FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and Lottery.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
					and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(Value) as TotalDirectCPLAwardedInLottery
		FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
		WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
				and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	+
	ISNULL((SELECT SUM(Amount) as TotalDirectCPLUsedInPricePrediction
		FROM PricePredictionHistory 
		WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
	-
	ISNULL((SELECT SUM(TotalAward) as TotalDirectCPLAwardedInPricePrediction
		FROM PricePredictionHistory 
		WHERE   PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
				and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))),0)
AS DirectAffiliateSale,

--------------------------
-- Total Introduced Users --
--------------------------
	ISNULL((LEN(@DirectIntroducedUsersOnPeriod) - LEN(REPLACE(@DirectIntroducedUsersOnPeriod,',','')) + 1),0)
	+ 
	ISNULL((LEN(@Tier2IntroducedUsersOnPeriod) - LEN(REPLACE(@Tier2IntroducedUsersOnPeriod,',','')) + 1),0)
	+ 
	ISNULL((LEN(@Tier3IntroducedUsersOnPeriod) - LEN(REPLACE(@Tier3IntroducedUsersOnPeriod,',','')) + 1),0)
AS TotalIntroducedUsers,

---------------------------
-- Direct Introduced Users --
---------------------------
	ISNULL((LEN(@DirectIntroducedUsersOnPeriod) - LEN(REPLACE(@DirectIntroducedUsersOnPeriod,',','')) + 1),0)
AS DirectIntroducedUsers

FROM SysUser su 
WHERE su.Id = @SysUserId and su.AffiliateId is not null and su.AffiliateId > 0


--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--//////////////////////////// DATATABLE #2 -  SALE & INTRODUCED USERS GROUPED BY DATE ////////////////////////////////////--
--/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--  1.1.1 AFFILIATE SALE - DIRECT INTRODUCED USERS - USED CPL IN LOTTERY
DECLARE @TableDirectUsedCPLInLottery TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableDirectUsedCPLInLottery
SELECT	SUM(l.UnitPrice),
		CAST(lh.CreatedDate AS DATE)
FROM	 LotteryHistory lh join Lottery l on l.Id = lh.LotteryId
WHERE	lh.Result is not null and lh.Result <> 'REFUND' -- WIN / LOSE 
	and lh.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and lh.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
GROUP BY CAST(lh.CreatedDate AS DATE)

---------------------------------------------------------
------------------------- LOTTERY -----------------------
---------------------------------------------------------

--  1.1.2 AFFILIATE SALE - DIRECT INTRODUCED USERS - AWARDED CPL IN LOTTERY
DECLARE @TableDirectAwardedCPLInLottery TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableDirectAwardedCPLInLottery
SELECT	SUM(lp.Value), 
		CAST(lh.UpdatedDate AS DATE)
FROM	 LotteryHistory lh join LotteryPrize lp on lh.LotteryPrizeId = lp.Id
WHERE	lh.Result is not null and lh.Result <> 'REFUND' -- WIN / LOSE 
	and lh.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and lh.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
GROUP BY CAST(lh.UpdatedDate AS DATE)



--  1.2.1 AFFILIATE SALE - TIER 2 INTRODUCED USERS - USED CPL IN LOTTERY
DECLARE @TableTier2UsedCPLInLottery TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTier2UsedCPLInLottery
SELECT	SUM(l.UnitPrice),
		CAST(lh.CreatedDate AS DATE)
FROM	 LotteryHistory lh join Lottery l on l.Id = lh.LotteryId
WHERE	lh.Result is not null and lh.Result <> 'REFUND' -- WIN / LOSE 
	and lh.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and lh.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))
GROUP BY CAST(lh.CreatedDate AS DATE)

-- 1.2.2 AFFILIATE SALE - TIER 2 INTRODUCED USERS - AWARDED CPL IN LOTTERY
DECLARE @TableTier2AwardedCPLInLottery TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTier2AwardedCPLInLottery
SELECT	SUM(lp.Value), 
		CAST(lh.UpdatedDate AS DATE)
FROM	 LotteryHistory lh join LotteryPrize lp on lh.LotteryPrizeId = lp.Id
WHERE	lh.Result is not null and lh.Result <> 'REFUND' -- WIN / LOSE 
	and lh.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and lh.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))
GROUP BY CAST(lh.UpdatedDate AS DATE)



--  1.3.1 AFFILIATE SALE - TIER 3 INTRODUCED USERS - USED CPL IN LOTTERY
DECLARE @TableTier3UsedCPLInLottery TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTier3UsedCPLInLottery
SELECT	SUM(l.UnitPrice),
		CAST(lh.CreatedDate AS DATE)
FROM	 LotteryHistory lh join Lottery l on l.Id = lh.LotteryId
WHERE	lh.Result is not null and lh.Result <> 'REFUND' -- WIN / LOSE 
	and lh.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and lh.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))
GROUP BY CAST(lh.CreatedDate AS DATE)

--  1.3.2 AFFILIATE SALE - TIER 3 INTRODUCED USERS - AWARDED CPL IN LOTTERY
DECLARE @TableTier3AwardedCPLInLottery TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTier3AwardedCPLInLottery
SELECT	SUM(lp.Value), 
		CAST(lh.UpdatedDate AS DATE)
FROM	 LotteryHistory lh join LotteryPrize lp on lh.LotteryPrizeId = lp.Id
WHERE	lh.Result is not null and lh.Result <> 'REFUND' -- WIN / LOSE 
	and lh.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and lh.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))
GROUP BY CAST(lh.UpdatedDate AS DATE)


---------------------------------------------------------
-------------------- PRICE PREDICTION -------------------
---------------------------------------------------------
--  2.1.1 AFFILIATE SALE - DIRECT INTRODUCED USERS - USED CPL IN PRICE PREDICTION
DECLARE @TableDirectUsedCPLInPricePrediction TABLE
(
	Value money, 
	Date datetime
);

INSERT	INTO @TableDirectUsedCPLInPricePrediction
SELECT	SUM(pph.Amount),
		CAST(pph.CreatedDate AS DATE)
FROM	PricePredictionHistory pph 
		WHERE	pph.Result is not null and pph.Result <> 'REFUND' -- WIN / LOSE 
				and pph.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and pph.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
GROUP BY CAST(pph.CreatedDate AS DATE)

--  2.1.2 AFFILIATE SALE - DIRECT INTRODUCED USERS - AWARDED CPL IN PRICE PREDICTION
DECLARE @TableDirectAwardedCPLInPricePrediction TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableDirectAwardedCPLInPricePrediction
SELECT	SUM(pph.TotalAward),
		CAST(pph.UpdatedDate AS DATE)
FROM	PricePredictionHistory pph 
		WHERE	pph.Result is not null and pph.Result <> 'REFUND' -- WIN / LOSE 
				and pph.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and pph.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
GROUP BY CAST(pph.UpdatedDate AS DATE)


--  2.2.1 AFFILIATE SALE - TIER 2 INTRODUCED USERS - USED CPL IN PRICE PREDICTION
DECLARE @TableTier2UsedCPLInPricePrediction TABLE
(
	Value money, 
	Date datetime
);

INSERT	INTO @TableTier2UsedCPLInPricePrediction
SELECT	SUM(pph.Amount),
		CAST(pph.CreatedDate AS DATE)
FROM	PricePredictionHistory pph 
		WHERE	pph.Result is not null and pph.Result <> 'REFUND' -- WIN / LOSE 
				and pph.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and pph.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))
GROUP BY CAST(pph.CreatedDate AS DATE)

--  2.2.2 AFFILIATE SALE - TIER 2 INTRODUCED USERS - AWARDED CPL IN PRICE PREDICTION
DECLARE @TableTier2AwardedCPLInPricePrediction TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTier2AwardedCPLInPricePrediction
SELECT	SUM(pph.TotalAward),
		CAST(pph.UpdatedDate AS DATE)
FROM	PricePredictionHistory pph 
		WHERE	pph.Result is not null and pph.Result <> 'REFUND' -- WIN / LOSE 
				and pph.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and pph.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))
GROUP BY CAST(pph.UpdatedDate AS DATE)



--  2.3.1 AFFILIATE SALE - TIER 3 INTRODUCED USERS - USED CPL IN PRICE PREDICTION
DECLARE @TableTier3UsedCPLInPricePrediction TABLE
(
	Value money, 
	Date datetime
);

INSERT	INTO @TableTier3UsedCPLInPricePrediction
SELECT	SUM(pph.Amount),
		CAST(pph.CreatedDate AS DATE)
FROM	PricePredictionHistory pph 
		WHERE	pph.Result is not null and pph.Result <> 'REFUND' -- WIN / LOSE 
				and pph.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and pph.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))
GROUP BY CAST(pph.CreatedDate AS DATE)

--  2.3.2 AFFILIATE SALE - TIER 3 INTRODUCED USERS - AWARDED CPL IN PRICE PREDICTION
DECLARE @TableTier3AwardedCPLInPricePrediction TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTier3AwardedCPLInPricePrediction
SELECT	SUM(pph.TotalAward),
		CAST(pph.UpdatedDate AS DATE)
FROM	PricePredictionHistory pph 
		WHERE	pph.Result is not null and pph.Result <> 'REFUND' -- WIN / LOSE 
				and pph.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
				and pph.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier3IntroducedUsers, ','))
GROUP BY CAST(pph.UpdatedDate AS DATE)


DECLARE @TableTotalAffiliateSale TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableTotalAffiliateSale
SELECT SUM(_Value), _Date
FROM (
	SELECT	ISNULL(dulottery.Value, 0) - ISNULL(dalottery.Value, 0) 
			+ 
			ISNULL(t2ulottery.Value, 0) - ISNULL(t2alottery.Value, 0) 
			+
			ISNULL(t3ulottery.Value, 0) - ISNULL(t3alottery.Value, 0) 
			+
			ISNULL(dupriceprediction.Value, 0) - ISNULL(dapriceprediction.Value, 0) 
			+ 
			ISNULL(t2upriceprediction.Value, 0) - ISNULL(t2apriceprediction.Value, 0) 
			+
			ISNULL(t3upriceprediction.Value, 0) - ISNULL(t3apriceprediction.Value, 0) 
			as _Value, 
			COALESCE(dulottery.Date, dalottery.Date, t2ulottery.Date, t2alottery.Date, t3ulottery.Date, t3alottery.Date,
					  dupriceprediction.Date, dapriceprediction.Date, t2upriceprediction.Date, t2apriceprediction.Date, t3upriceprediction.Date, t3apriceprediction.Date) as _Date
	FROM	@TableDirectUsedCPLInLottery dulottery
			full outer join @TableDirectAwardedCPLInLottery dalottery on dulottery.Date = dalottery.Date
			full outer join @TableTier2UsedCPLInLottery t2ulottery on dulottery.Date = t2ulottery.Date
			full outer join @TableTier2AwardedCPLInLottery t2alottery on dulottery.Date = t2alottery.Date
			full outer join @TableTier3UsedCPLInLottery t3ulottery on dulottery.Date = t3ulottery.Date
			full outer join @TableTier3AwardedCPLInLottery t3alottery on dulottery.Date = t3alottery.Date
			full outer join @TableDirectUsedCPLInPricePrediction dupriceprediction on dulottery.Date = dupriceprediction.Date
			full outer join @TableDirectAwardedCPLInPricePrediction dapriceprediction on dulottery.Date = dapriceprediction.Date
			full outer join @TableTier2UsedCPLInPricePrediction t2upriceprediction on dulottery.Date = t2upriceprediction.Date
			full outer join @TableTier2AwardedCPLInPricePrediction t2apriceprediction on dulottery.Date = t2apriceprediction.Date
			full outer join @TableTier3UsedCPLInPricePrediction t3upriceprediction on dulottery.Date = t3upriceprediction.Date
			full outer join @TableTier3AwardedCPLInPricePrediction t3apriceprediction on dulottery.Date = t3apriceprediction.Date
	GROUP BY COALESCE(dulottery.Date, dalottery.Date, t2ulottery.Date, t2alottery.Date, t3ulottery.Date, t3alottery.Date,
					  dupriceprediction.Date, dapriceprediction.Date, t2upriceprediction.Date, t2apriceprediction.Date, t3upriceprediction.Date, t3apriceprediction.Date), 
			ISNULL(dulottery.Value, 0) - ISNULL(dalottery.Value, 0) 
			+ 
			ISNULL(t2ulottery.Value, 0) - ISNULL(t2alottery.Value, 0) 
			+
			ISNULL(t3ulottery.Value, 0) - ISNULL(t3alottery.Value, 0) 
			+
			ISNULL(dupriceprediction.Value, 0) - ISNULL(dapriceprediction.Value, 0) 
			+ 
			ISNULL(t2upriceprediction.Value, 0) - ISNULL(t2apriceprediction.Value, 0) 
			+
			ISNULL(t3upriceprediction.Value, 0) - ISNULL(t3apriceprediction.Value, 0) 
) AS TableTotalAffiliateSale
GROUP BY _Date

DECLARE @TableDirectAffiliateSale TABLE
(
	Value money, 
	Date datetime
);

INSERT INTO @TableDirectAffiliateSale
SELECT SUM(Value), Date
FROM (
	SELECT	ISNULL(dulottery.Value, 0) - ISNULL(dalottery.Value, 0) 
			+
			ISNULL(dupriceprediction.Value, 0) - ISNULL(dapriceprediction.Value, 0) 
			as Value, 
			COALESCE(dulottery.Date, dalottery.Date, dupriceprediction.Date, dapriceprediction.Date) as Date
	FROM	@TableDirectUsedCPLInLottery dulottery
			full outer join @TableDirectAwardedCPLInLottery dalottery on dulottery.Date = dalottery.Date
			full outer join @TableDirectUsedCPLInPricePrediction dupriceprediction on dulottery.Date = dupriceprediction.Date
			full outer join @TableDirectAwardedCPLInPricePrediction dapriceprediction on dulottery.Date = dapriceprediction.Date
	GROUP BY COALESCE(dulottery.Date, dalottery.Date, dupriceprediction.Date, dapriceprediction.Date), 
			ISNULL(dulottery.Value, 0) - ISNULL(dalottery.Value, 0) 
			+
			ISNULL(dupriceprediction.Value, 0) - ISNULL(dapriceprediction.Value, 0) 
) AS TableDirectAffiliateSale
GROUP BY Date



--  3.1 NUMBER - DIRECT INTRODUCED USERS
DECLARE @TableDirectIntroducedUsers TABLE
(
	Value int, 
	Date datetime
);

INSERT INTO @TableDirectIntroducedUsers
SELECT	COUNT(Id), CAST(CreatedDate AS DATE)
		FROM	SysUser 
		WHERE	CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
			and IsIntroducedById =  @SysUserId
GROUP BY CAST(CreatedDate AS DATE)

--  3.2 NUMBER - TIER 2 INTRODUCED USERS
DECLARE @TableTier2IntroducedUsers TABLE
(
	Value int, 
	Date datetime
);

INSERT INTO @TableTier2IntroducedUsers
SELECT	COUNT(Id), CAST(CreatedDate AS DATE)
		FROM	SysUser 
		WHERE	CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
			and SysUser.IsIntroducedById in (
				SELECT	Id
				FROM	SysUser 
				WHERE	SysUser.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
					and SysUser.IsIntroducedById =  @SysUserId)
GROUP BY CAST(CreatedDate AS DATE)

--  3.3 NUMBER - TIER 3 INTRODUCED USERS
DECLARE @TableTier3IntroducedUsers TABLE
(
	Value int, 
	Date datetime
);

INSERT INTO @TableTier3IntroducedUsers
SELECT	COUNT(Id), CAST(CreatedDate AS DATE)
		FROM	SysUser 
		WHERE	CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
			and SysUser.IsIntroducedById in (
				SELECT	Id
				FROM	SysUser 
				WHERE	SysUser.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
					and SysUser.IsIntroducedById in (
						SELECT	Id
						FROM	SysUser 
						WHERE	SysUser.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
							and SysUser.IsIntroducedById =  @SysUserId))
GROUP BY CAST(CreatedDate AS DATE)

DECLARE @TableTotalIntroducedUsers TABLE
(
	Value int, 
	Date datetime
);


INSERT INTO @TableTotalIntroducedUsers
SELECT SUM(Value), Date
FROM (
	SELECT	ISNULL(diu.Value, 0) + ISNULL(t2iu.Value, 0) + ISNULL(t3iu.Value, 0)
			as Value, 
			COALESCE(diu.Date, t2iu.Date, t3iu.Date) as Date
	FROM	@TableDirectIntroducedUsers diu
			full outer join @TableTier2IntroducedUsers t2iu on diu.Date = t2iu.Date
			full outer join @TableTier3IntroducedUsers t3iu on diu.Date = t3iu.Date
	GROUP BY ISNULL(diu.Value, 0) + ISNULL(t2iu.Value, 0) + ISNULL(t3iu.Value, 0),
			COALESCE(diu.Date, t2iu.Date, t3iu.Date)
) TotalIntrocuedUsers
GROUP BY Date


------------------------ FINALIZE THEM ALL ------------------------
SELECT	SUM(ISNULL(tas.Value, 0)) as TotalAffiliateSale, 
		SUM(ISNULL(das.Value, 0)) as DirectAffiliateSale, 
		SUM(ISNULL(tiu.Value, 0)) as TotalIntroducedUsers, 
		SUM(ISNULL(diu.Value, 0)) as DirectIntroducedUsers, 
		COALESCE(das.Date, tas.Date, tiu.Date, diu.Date) as Date
FROM	@TableDirectAffiliateSale das
		full outer join @TableTotalAffiliateSale tas on das.Date = tas.Date
		full outer join @TableTotalIntroducedUsers tiu on das.Date = tiu.Date
		full outer join @TableDirectIntroducedUsers diu on das.Date = diu.Date
GROUP BY COALESCE(das.Date, tas.Date, tiu.Date, diu.Date)

--SELECT * 
--FROM @TableTotalAffiliateSale;

--SELECT *
--FROM @TableDirectAffiliateSale;

--SELECT * 
--FROM @TableDirectIntroducedUsers

--SELECT * 
--FROM @TableTier2IntroducedUsers

--SELECT * 
--FROM @TableTier3IntroducedUsers

--SELECT *
--FROM @TableTotalIntroducedUsers;
END	
