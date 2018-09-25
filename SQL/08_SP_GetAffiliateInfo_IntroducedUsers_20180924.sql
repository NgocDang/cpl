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
CREATE PROCEDURE [dbo].[usp_GetAffiliateInfo_IntroducedUsers]
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
--///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////--
--////////////////////////////// DATATABLE #1 - INTRODUCED TIER 2 & TIER 3 USERS IN DETAILS /////////////////////////////--
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

SELECT @DirectIntroducedUsers = STRING_AGG(Id, ',')
FROM SysUser Tier1
WHERE Tier1.IsIntroducedById = @SysUserId and Tier1.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())

SELECT @Tier2IntroducedUsers = STRING_AGG(Id, ',') 
FROM SysUser Tier2 
WHERE Tier2.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
	and Tier2.IsIntroducedById in 
		(SELECT Id 
		 FROM SysUser Tier1
		 WHERE Tier1.IsIntroducedById = @SysUserId and Tier1.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate()))

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
	TotalDirectIntroducedUsers int,
	AffiliateCreatedDate datetime,
	Tier1DirectRate int,
	Tier2SaleToTier1Rate int,
	Tier3SaleToTier1Rate int,
	AffiliateId int,
	IsLocked bit,
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
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId = su.Id),0)
		+
		ISNULL((SELECT SUM(Amount) 
				FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
				WHERE PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId = su.Id),0)
		As UsedCPL,
								 
		-------------
		-- LostCPL --
		-------------
		ISNULL((SELECT SUM(UnitPrice)
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId = su.Id),0)
		-
		ISNULL((SELECT SUM(LotteryPrize.Value)
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId = LotteryPrize.Id
				WHERE LotteryHistory.Result = 'WIN' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId = su.Id),0)
		+
		ISNULL((SELECT SUM(Amount) 
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE 
						and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId = su.Id),0)
		-
		ISNULL((SELECT SUM(TotalAward) 
				FROM PricePredictionHistory
				WHERE PricePredictionHistory.Result = 'WIN' -- WIN / LOSE 
						and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
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
						and LotteryHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT DirectIntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			---------------------------
			-- Tier 2 affiliate sale --
			---------------------------
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier2IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			---------------------------
			-- Tier 3 affiliate sale --
			---------------------------
			ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
				FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
				FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
				WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
						and LotteryHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and LotteryHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			+
			ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
			-
			ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
				FROM PricePredictionHistory 
				WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
						and PricePredictionHistory.UpdatedDate >= DATEADD(d, -@PeriodInDay, getdate())
						and PricePredictionHistory.SysUserId in (SELECT CAST(Value AS int) FROM STRING_SPLIT((SELECT Tier3IntroducedUsers FROM IntroducedUsers WHERE IntroducedUsers.Id = su.Id), ','))),0)
		--				)) AffiliateSale(Value))
		As AffiliateSale,

	----------------------------
	-- Direct introduced users --
	----------------------------
	   (SELECT COUNT(Id) FROM SysUser WHERE SysUser.IsIntroducedById = su.Id and SysUser.CreatedDate >= DATEADD(d, -@PeriodInDay, getdate()))
		AS TotalDirectIntroducedUsers,

	----------------------------
	-- Affiliate created date --
	----------------------------
		su.AffiliateCreatedDate
		AS AffiliateCreatedDate,

	------------------------
	-- Tier 1 Direct Rate --
	------------------------
		aff.Tier1DirectRate
		AS Tier1DirectRate,

	---------------------------
	-- Tier 2 to Tier 1 Rate --
	---------------------------
		aff.Tier2SaleToTier1Rate
		AS Tier2SaleToTier1Rate,

	---------------------------
	-- Tier 3 to Tier 2 Rate --
	---------------------------
		aff.Tier3SaleToTier1Rate
		AS Tier3SaleToTier1Rate,
	
	------------------
	-- Affiliate Id --
	------------------
		su.AffiliateId
		AS AffiliateId,

	---------------
	-- Is Locked --
	---------------
		su.IsLocked
		AS IsLocked

	FROM   SysUser su join IntroducedUsers iu on su.Id = iu.Id
					  join Affiliate aff on su.AffiliateId = aff.Id
	WHERE (su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@DirectIntroducedUsers, ','))
		or su.Id in (SELECT CAST(Value AS int) FROM STRING_SPLIT(@Tier2IntroducedUsers, ',')))
		and su.AffiliateId is not null 
		and su.AffiliateId > 0
),

------------------------------------------------------------------------------------------
---------------------------- 2.2 APPLY SORT / SEARCH / PAGING  --------------------------- 
------------------------------------------------------------------------------------------

IntroducedUsersWithRowNumCTE AS
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
				 WHEN @OrderColumn = N'TotalDirectIntroducedUsers' THEN TotalDirectIntroducedUsers
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
				 WHEN @OrderColumn = N'TotalDirectIntroducedUsers' THEN TotalDirectIntroducedUsers
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
SELECT Id, KindOfTier, UsedCPL, LostCPL, AffiliateSale, TotalDirectIntroducedUsers, AffiliateCreatedDate, Tier1DirectRate, Tier2SaleToTier1Rate, Tier3SaleToTier1Rate, AffiliateId, IsLocked, RowNum
FROM IntroducedUsersWithRowNumCTE;

SELECT Id, KindOfTier, UsedCPL, LostCPL, AffiliateSale, TotalDirectIntroducedUsers, AffiliateCreatedDate, Tier1DirectRate, Tier2SaleToTier1Rate, Tier3SaleToTier1Rate, AffiliateId, IsLocked
FROM @TableIntroducedUsers
WHERE RowNum  BETWEEN ((@PageIndex - 1) * @PageSize + 1) AND (@PageIndex * @PageSize);

--///////////////////////////////////////////////////////////////////////////////////////--
--////////////////////////////// DATATABLE #2 - Total Count /////////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////--


SELECT TotalDirectIntroducedUsers + TotalTier2IntroducedUsers as TotalCount
FROM IntroducedUsers
WHERE Id = @SysUserId

--///////////////////////////////////////////////////////////////////////////////////////--
--////////////////////////////// DATATABLE #3 - Filtered Count //////////////////////////--
--///////////////////////////////////////////////////////////////////////////////////////--

SELECT COUNT(*) as FilteredCount
FROM   @TableIntroducedUsers
END
