CREATE VIEW IntroducedUsers AS
(SELECT 
		Id, 
		-- Direct introduced users
		(SELECT STRING_AGG(Id, ',') AS DirectIntroducedUsers
		FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id) as DirectIntroducedUsers,

		-- Tier 2 introduced users
		(SELECT STRING_AGG(Id, ',') AS Tier2IntroducedUsers
		FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
			(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id)) as Tier2IntroducedUsers,
		-- Tier 3 introduced users
		(SELECT STRING_AGG(Id, ',') AS Tier3IntroducedUsers
		FROM SysUser Tier3 WHERE Tier3.IsIntroducedById in 
					(SELECT Id FROM SysUser Tier2 WHERE Tier2.IsIntroducedById in 
						(SELECT Id FROM SysUser Tier1 WHERE Tier1.IsIntroducedById = su.Id))) as Tier3IntroducedUsers
	FROM SysUser su
	WHERE AffiliateId is not null and AffiliateId > 0)