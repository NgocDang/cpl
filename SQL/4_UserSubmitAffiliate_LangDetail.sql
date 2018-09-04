alter table SysUser add AffiliateCreatedDate datetime null;

insert into Setting values (N'IsKYCVerificationActivated', N'false',null);

insert into LangDetail values (1, N'BecomeAnAffiliate', N'Do you want to become an affiliate?');
insert into LangDetail values (2, N'BecomeAnAffiliate', N'あなたは、アフィリエイトに参加しますか？');

insert into LangDetail values (1, N'Join', N'Join');
insert into LangDetail values (2, N'Join', N'参加');

insert into LangDetail values (1, N'Affiliate', N'Affiliate');
insert into LangDetail values (2, N'Affiliate', N'アフィリエイト');

insert into LangDetail values (1, N'AffiliateApplicationReceived', N'We have received your affiliate application, please wait for our update.  ');
insert into LangDetail values (2, N'AffiliateApplicationReceived', N'あなたのアフィリエイトの申し込みを受け付けました。更新までしばらくお待ちください。');

insert into LangDetail values (1, N'AffiliateApplicationApproved', N'Your affiliate application is approved.');
insert into LangDetail values (2, N'AffiliateApplicationApproved', N'あなたのアフィリエイトの申し込みは承認されました。');

insert into LangDetail values (1, N'Approved', N'Approved');
insert into LangDetail values (2, N'Approved', N'承認済み');

insert into LangDetail values (1, N'KYCRequiredBeforeAffiliate', N'Please <a href=''/Profile/Kyc''>verify your KYC</a> before submitting affiliate application');
insert into LangDetail values (2, N'KYCRequiredBeforeAffiliate', N'アフィリエイトプログラムに応募する前に、<a href=''/Profile/Kyc''>KYCを提出</a>してください');

insert into LangDetail values (1, N'NotJoinedYet', N'not joined yet.');
insert into LangDetail values (2, N'NotJoinedYet', N'参加していません。');

insert into LangDetail values (1, N'ClickHereToChangeAffiliate', N'Click <a href="/Profile/Affiliate" class="text-primary">here</a> to change.');
insert into LangDetail values (2, N'ClickHereToChangeAffiliate', N'変更は <a href="/Profile/Affiliate" class="text-primary">ココ</a> をクリックしてください。');

insert into LangDetail values (1, N'AffiliateApplicationSubmitted', N'Your affiliate application is submitted successfully.');
insert into LangDetail values (2, N'AffiliateApplicationSubmitted', N'あなたの申し込みは完了しました。');


update LangDetail set Name = 'ClickHereToChangeKYC' where Id = 1699 or Id = 1861

