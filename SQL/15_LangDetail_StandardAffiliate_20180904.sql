Use CPL;

ALTER TABLE sysUser
ADD IsLocked bit NOT NULL Default(0)

INSERT INTO LangDetail VALUES (1, N'ListOfStandardAffiliate', N'List Of StandardAffiliate');
INSERT INTO LangDetail VALUES (2, N'ListOfStandardAffiliate', N'標準アフィリエイトのリスト');

INSERT INTO LangDetail VALUES (1, N'ListOfStandardAffiliate', N'List Of StandardAffiliate');
INSERT INTO LangDetail VALUES (2, N'ListOfStandardAffiliate', N'標準アフィリエイトのリスト');

INSERT INTO LangDetail VALUES (1, N'TotalSaleInCPL', N'Total sale in CPL');
INSERT INTO LangDetail VALUES (2, N'TotalSaleInCPL', N'CPLでの総販売額');

INSERT INTO LangDetail VALUES (1, N'TotalIntroducer', N'Total introducer');
INSERT INTO LangDetail VALUES (2, N'TotalIntroducer', N'トータルイントロデューサ');

INSERT INTO LangDetail VALUES (1, N'AffiliateCreatedDate', N'Affiliate created date');
INSERT INTO LangDetail VALUES (2, N'AffiliateCreatedDate', N'アフィリエイト作成日');

INSERT INTO LangDetail VALUES (1, N'Tier1', N'Tier 1');
INSERT INTO LangDetail VALUES (2, N'Tier1', N'ティア 1');

INSERT INTO LangDetail VALUES (1, N'Tier2', N'Tier 2');
INSERT INTO LangDetail VALUES (2, N'Tier2', N'ティア 2');

INSERT INTO LangDetail VALUES (1, N'Tier3', N'Tier 3');
INSERT INTO LangDetail VALUES (2, N'Tier3', N'ティア 3');

INSERT INTO LangDetail VALUES (1, N'Lock', N'Lock');
INSERT INTO LangDetail VALUES (2, N'Lock', N'ロック');

INSERT INTO LangDetail VALUES (1, N'UnLock', N'UnLock');
INSERT INTO LangDetail VALUES (2, N'UnLock', N'アンロック');


