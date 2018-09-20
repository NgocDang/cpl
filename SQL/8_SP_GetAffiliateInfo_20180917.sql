GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_GetAffiliateInfo]
	-- Add the parameters for the stored procedure here
	@SysUserId int,
	@PeriodInDay int,
	@PageSize int,
	@PageIndex int,
	@OrderColumn nvarchar(30),
	@OrderDirection nvarchar(5),
	@SearchValue nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here

--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--/////////////////////////////////// DATATABLE #1 -  SALE & INTRODUCED USERS ///////////////////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--

	SELECT 
		--------------------------------------------------
		-- Affiliate Sale | Tier 1 Direct Afiliate Sale --
		--------------------------------------------------
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
		--------------------------------------------------------------------
		-- Affiliate Sale | Tier 2 Affiliate Sale to Tier 1 Afiliate Sale --
		--------------------------------------------------------------------
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
		--------------------------------------------------------------------
		-- Affiliate Sale | Tier 3 Affiliate Sale to Tier 1 Afiliate Sale --
		--------------------------------------------------------------------
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

		---------------------------
		-- Direct Affiliate Sale --
		---------------------------
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
		-- Total Introduced Users --
		--------------------------
			ISNULL((LEN(DirectIntroducedUsers) - LEN(REPLACE(DirectIntroducedUsers,',','')) + 1),0)
			+ 
			ISNULL((LEN(Tier2IntroducedUsers) - LEN(REPLACE(Tier2IntroducedUsers,',','')) + 1),0)
			+ 
			ISNULL((LEN(Tier3IntroducedUsers) - LEN(REPLACE(Tier3IntroducedUsers,',','')) + 1),0)
		AS TotalIntroducedUsers,

		---------------------------
		-- Direct Introduced Users --
		---------------------------
			ISNULL((LEN(DirectIntroducedUsers) - LEN(REPLACE(DirectIntroducedUsers,',','')) + 1),0)
		AS DirectIntroducedUsers
	FROM SysUser su join IntroducedUsers iu on su.Id = iu.Id
	WHERE su.Id = @SysUserId and su.AffiliateId is not null and su.AffiliateId > 0







--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--////////////////////////////// DATATABLE #2 - INTRODUCED TIER 2 & TIER 3 USERS IN DETAILS /////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--

-------------------- BEGIN SETTING PARAM FOR TESTING PURPOSE --------------------
	--DECLARE @SysUserId int;
	--SET @SysUserId = 1;

	--DECLARE @PeriodInDay int;
	--SET @PeriodInDay = 300;
-------------------- END SETTING PARAM FOR TESTING PURPOSE --------------------

	DECLARE @DirectIntroducedUsers nvarchar(MAX);
	DECLARE @Tier2IntroducedUsers nvarchar(MAX);
	DECLARE @FilteredCount int;
	DECLARE @TotalCount int;
	SELECT  @DirectIntroducedUsers = DirectIntroducedUsers, 
			@Tier2IntroducedUsers = Tier2IntroducedUsers 
	FROM	IntroducedUsers
	WHERE	Id = @SysUserId;

------------------------------------------------------------------------------------
-------------------------- 2.1 CONSTRUCT SQL QUERY FIRST --------------------------- 
------------------------------------------------------------------------------------
	DECLARE @TableIntroducedUsers TABLE
	(
		Id int, 
		KindOfTier nvarchar(20),
		UsedCPL money,
		LostCPL money,
		AffiliateSale money,
		TotalIntroducedUsers int,
		AffiliateCreatedDate datetime,
		RowNum int
	);

WITH IntroducedUsersCTE AS 
(
	SELECT 
		--------
		-- Id --
		--------
		su.Id,

		----------------
		-- KindOfTier --
		----------------
		CASE WHEN su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
				THEN 'Tier 2' -- Tier 2
				WHEN su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ','))
				THEN 'Tier 3' -- Tier 3
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
		--				)) AffiliateSale(Value))
		As AffiliateSale,

	----------------------------
	-- Total introduced users --
	----------------------------
		ISNULL((LEN((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id)) - LEN(REPLACE((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id),',','')) + 1),0)
		AS TotalIntroducedUsers,

	----------------------------
	-- Affiliate created date --
	----------------------------
		su.AffiliateCreatedDate
		AS AffiliateCreatedDate

	FROM   SysUser su join IntroducedUsers iu on su.Id = iu.Id
	WHERE (su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
		or su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ',')))
		and su.AffiliateId is not null 
		and su.AffiliateId > 0
),

------------------------------------------------------------------------------------------
---------------------------- 2.2 APPLY SORT / SEARCH / PAGING  --------------------------- 
------------------------------------------------------------------------------------------

IntroducedUsersWithRowNum AS
(	
	SELECT *, 
		RowNum = ROW_NUMBER() OVER (
			ORDER BY -- ASC
			CASE WHEN @OrderDirection = N'asc' THEN ''
				 WHEN @OrderColumn = N'KindOfTier' THEN KindOfTier
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
				 WHEN @OrderColumn = N'UsedCPL' THEN UsedCPL
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
				 WHEN @OrderColumn = N'LostCPL' THEN LostCPL
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as money)
				 WHEN @OrderColumn = N'AffiliateSale' THEN AffiliateSale
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN 0
				 WHEN @OrderColumn = N'TotalIntroducedUsers' THEN TotalIntroducedUsers
				 END DESC,
			CASE WHEN @OrderDirection = N'asc' THEN cast(null as datetime)
				 WHEN @OrderColumn = N'AffiliateCreatedDate' THEN AffiliateCreatedDate
				 END DESC,

				-- DESC
			CASE WHEN @OrderDirection = N'desc' THEN ''
				 WHEN @OrderColumn = N'KindOfTier' THEN KindOfTier
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
				 WHEN @OrderColumn = N'UsedCPL' THEN UsedCPL
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
				 WHEN @OrderColumn = N'LostCPL' THEN LostCPL
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as money)
				 WHEN @OrderColumn = N'AffiliateSale' THEN AffiliateSale
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN 0
				 WHEN @OrderColumn = N'TotalIntroducedUsers' THEN TotalIntroducedUsers
				 END ASC,
			CASE WHEN @OrderDirection = N'desc' THEN cast(null as datetime)
				 WHEN @OrderColumn = N'AffiliateCreatedDate' THEN AffiliateCreatedDate
				 END ASC)
	FROM IntroducedUsersCTE
	WHERE(KindOfTier like '%' + @SearchValue + '%'
		  OR 
		  CONVERT(nvarchar(23), AffiliateCreatedDate, 0) like ('%' + @SearchValue + '%'))
	)
	INSERT INTO @TableIntroducedUsers
	SELECT Id, KindOfTier, UsedCPL, LostCPL, AffiliateSale, TotalIntroducedUsers, AffiliateCreatedDate, RowNum
	FROM IntroducedUsersWithRowNum;
	
	SELECT * 
	FROM @TableIntroducedUsers
	WHERE RowNum  BETWEEN ((@PageIndex - 1) * @PageSize + 1) AND (@PageIndex * @PageSize);

	SELECT TotalDirectIntroducedUsers + TotalTier2IntroducedUsers as TotalCount
	FROM IntroducedUsers
	WHERE Id = @SysUserId

	SELECT COUNT(*) as FilteredCount
	FROM   @TableIntroducedUsers
END
