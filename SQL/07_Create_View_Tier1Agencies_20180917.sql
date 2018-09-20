CREATE VIEW Tier1Agencies AS
(SELECT Id 
	FROM SysUser 
	WHERE AffiliateId is not null and AffiliateId > 0 and AgencyId is not null and IsIntroducedById is null)

