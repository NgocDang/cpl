use CPL;



update Setting set Name = 'StandardAffiliate.Tier1DirectRate' where Id = 13;
update Setting set Name = 'StandardAffiliate.Tier2SaleToTier1Rate' where Id = 14;
update Setting set Name = 'StandardAffiliate.Tier3SaleToTier1Rate' where Id = 15;

update Setting set Name = 'AgencyAffiliate.Tier1DirectRate' where Id = 16;
update Setting set Name = 'AgencyAffiliate.Tier2DirectRate' where Id = 17;
update Setting set Name = 'AgencyAffiliate.Tier3DirectRate' where Id = 18;
update Setting set Name = 'AgencyAffiliate.Tier2SaleToTier1Rate' where Id = 19;
update Setting set Name = 'AgencyAffiliate.Tier3SaleToTier1Rate' where Id = 20;
update Setting set Name = 'AgencyAffiliate.Tier3SaleToTier2Rate' where Id = 21;

insert into Template values (N'AffiliateApprove', N'AffiliateApproveEmailTemplate', N'Affiliate application is approved at CPL');