CREATE VIEW IntroducedUsers AS
(SELECT 
		Id, 
		-- Direct introduced users
		(SELECT STRING_AGG(Id, ',') AS DirectIntroducedUsers
		FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id) as DirectIntroducedUsers,

		(SELECT COUNT(*) AS TotalDirectIntroducedUsers
		FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id) as TotalDirectIntroducedUsers,

		-- Tier 2 introduced users
		(SELECT STRING_AGG(Id, ',') AS Tier2IntroducedUsers
		FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
			(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)) as Tier2IntroducedUsers,

		(SELECT COUNT(*) AS TotalTier2IntroducedUsers
		FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
			(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)) as TotalTier2IntroducedUsers,

		-- Tier 3 introduced users
		(SELECT STRING_AGG(Id, ',') AS Tier3IntroducedUsers
		FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
					(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
						(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))) as Tier3IntroducedUsers,

		(SELECT COUNT(*) AS TotalTier3IntroducedUsers
		FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
					(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
						(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))) as TotalTier3IntroducedUsers,

		-- Affiliate sale
		---------------------------
		-- Direct affiliate sale --
		---------------------------
		----- Lottery
		ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
			FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.SysUserId in (SELECT Id AS DirectIntroducedUsers
													 FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)),0)
		-
		ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
			FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.SysUserId in (SELECT Id AS DirectIntroducedUsers
													 FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)),0)
		-- Plus Price prediction
			+
		ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
			FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
			WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
					and PricePredictionHistory.SysUserId in (SELECT Id AS DirectIntroducedUsers
													 FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)),0)
		-
		ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
			FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
			WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
					and PricePredictionHistory.SysUserId in (SELECT Id AS DirectIntroducedUsers
													 FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)),0)
		as DirectAffiliateSale,
		---------------------------
		-- Tier 2 affiliate sale --
		---------------------------
		----- Lottery
		ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
			FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.SysUserId in (SELECT Id AS Tier2IntroducedUsers
													 FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
															(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))),0)
		-
		ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
			FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.SysUserId in (SELECT Id AS Tier2IntroducedUsers
													 FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
															(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))),0)
		-- Plus Price prediction
			+
		----- Price prediction
		ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
			FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
			WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
					and PricePredictionHistory.SysUserId in (SELECT Id AS Tier2IntroducedUsers
													 FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
															(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))),0)
		-
		ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
			FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
			WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
					and PricePredictionHistory.SysUserId in (SELECT Id AS Tier2IntroducedUsers
													 FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
															(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))),0)
		as Tier2AffiliateSale,
		---------------------------
		-- Tier 3 affiliate sale --
		---------------------------
		----- Lottery
		ISNULL((SELECT SUM(UnitPrice) as TotalCPLUsedInLottery
			FROM LotteryHistory join Lottery on LotteryHistory.LotteryId = Lottery.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.SysUserId in (SELECT Id AS Tier3IntroducedUsers
													 FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
																(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
																	(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)))),0)
		-
		ISNULL((SELECT SUM(Value) as TotalCPLAwardedInLottery
			FROM LotteryHistory join LotteryPrize on LotteryHistory.LotteryPrizeId  = LotteryPrize.Id
								join Lottery on LotteryHistory.LotteryId = Lottery.Id
			WHERE LotteryHistory.Result is not null and LotteryHistory.Result <> 'REFUND' -- WIN / LOSE 
					and LotteryHistory.SysUserId in (SELECT Id AS Tier3IntroducedUsers
													 FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
																(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
																	(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)))),0)
		-- Plus Price prediction
			+
		----- Price prediction
		ISNULL((SELECT SUM(Amount) as TotalCPLUsedInPricePrediction
			FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
			WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
					and PricePredictionHistory.SysUserId in (SELECT Id AS Tier3IntroducedUsers
													 FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
																(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
																	(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)))),0)
		-
		ISNULL((SELECT SUM(TotalAward) as TotalCPLAwardedInPricePrediction
			FROM PricePredictionHistory join PricePrediction on PricePredictionHistory.PricePredictionId = PricePrediction.Id
			WHERE PricePredictionHistory.Result is not null and PricePredictionHistory.Result <> 'REFUND' -- WIN / LOSE
					and PricePredictionHistory.SysUserId in (SELECT Id AS Tier3IntroducedUsers
													 FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
																(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
																	(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)))),0)
	As Tier3AffiliateSale

	FROM SysUser su
	WHERE AffiliateId is not null and AffiliateId > 0)