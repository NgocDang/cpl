--- LAN - Home rebuild - 2018/09/28
use CPL;

insert into LangDetail values (1, N'PartnerCompanies', N'Partner games companies');
insert into LangDetail values (2, N'PartnerCompanies', N'提携ゲーム会社');

insert into LangDetail values (1, N'License', N'License');
insert into LangDetail values (2, N'License', N'ライセンス');

insert into LangDetail values (1, N'WhatIsBitcoin', N'What is Bitcoin?');
insert into LangDetail values (2, N'WhatIsBitcoin', N'ビットコインとは');

insert into LangDetail values (1, N'PersonalInformationProtection', N'Personal information protection');
insert into LangDetail values (2, N'PersonalInformationProtection', N'個人情報保護について');

select * from LangDetail where Name like '%About%'