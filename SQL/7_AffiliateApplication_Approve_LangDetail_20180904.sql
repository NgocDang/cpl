use CPL;

insert into LangDetail values (1, N'AffiliateApprove', N'Affiliate approve');
insert into LangDetail values (2, N'AffiliateApprove', N'アフィリエイト承認');

insert into LangDetail values (1, N'Approve', N'Approve');
insert into LangDetail values (2, N'Approve', N'承認する');

insert into LangDetail values (1, N'ListOfAffiliateApplication', N'List of affiliate application');
insert into LangDetail values (2, N'ListOfAffiliateApplication', N'アフィリエイトアプリケーション一覧');

insert into LangDetail values (1, N'ApplicationArePending', N'applications are pending');
insert into LangDetail values (2, N'ApplicationArePending', N'アプリケーションは保留中です');

insert into LangDetail values (1, N'AffiliateApplication', N'Affiliate application');
insert into LangDetail values (2, N'AffiliateApplication', N'アフィリエイトアプリケーション');

insert into LangDetail values (1, N'AffiliateApprovedDescription', N'Congratulation, your affiliate application is approved.');
insert into LangDetail values (2, N'AffiliateApprovedDescription', N'おめでとう、あなたのアフィリエイトアプリケーションが承認されました。');

insert into LangDetail values (1, N'AffiliateIsApproved', N'Affiliate application is approved successfully.');
insert into LangDetail values (2, N'AffiliateIsApproved', N'アフィリエイトアプリケーションが正常に承認されました。');

insert into LangDetail values (1, N'AffiliateHasBeenApproved', N'Affiliate application has been approved.');
insert into LangDetail values (2, N'AffiliateHasBeenApproved', N'アフィリエイトアプリケーションが承認されました。');


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