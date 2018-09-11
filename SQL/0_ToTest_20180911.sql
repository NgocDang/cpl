use CPL;
truncate table Affiliate;
truncate table Agency;
truncate table AgencyToken;
truncate table BTCPrice;

truncate table BTCTransaction;
truncate table CoinTransaction;

truncate table SysUser;
insert into SysUser values ('True', N'info.cplcoin@gmail.com', N'$2y$10$Sh.jL/v.yLPKj.2VqOrABOIwRvjCMKKpIWjFmRP9vyEwFgdvG9KhW', 
null, null, null,null,null,null, '2018-08-28 20:12:59.967', null, null, null, null, 1, null, 'False', 
N'0x37bd726d30e87040ce4e309a322914a1AFe0F2c4', 1, N'mrmdob16nnxGRfbC9jb683WiwaE1DTYEvA', 0, 0, 0, null, null, null, null, null, 
'False', null, null, null, null, 'False');

update Setting set Value = N'http://202.53.150.20/' where Name = 'CPLServiceEndPointUrl';

