CREATE VIEW Tier2Agencies AS
(SELECT Id 
	FROM SysUser 
	WHERE AffiliateId is not null and AffiliateId > 0 and AgencyId is not null and IsIntroducedById in (select Id from Tier1Agencies))

