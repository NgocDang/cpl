--- LAN - Home rebuild - 2018/09/28
use CPL;

update LangDetail set Name = N'AboutCPO', Value = N'About CPO' where Name = N'AboutCPL' and LangId = 1;
update LangDetail set Name = N'AboutCPO', Value = N'CPOについて' where Name = N'AboutCPL' and LangId = 2;

delete LangDetail where Name = 'WhatIsCPL';
delete LangMsgDetail where Name = 'WhatIsCPL';
											
-- DamTran - 20181009 - Update Lang Detail FOR Slider
UPDATE LangDetail 
SET [Value] = N'編集'
WHERE [Name] = N'Edit'
AND LangId = 2;