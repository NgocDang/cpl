use CPL;
alter table Lottery add SlideImage nvarchar(200) default '';
alter table Lottery add DesktopListingImage nvarchar(200) default '';
alter table Lottery add MobileListingImage nvarchar(200) default '';
alter table Lottery add Title nvarchar(200) default '';

insert into Lottery values (5, '2018-08-06 00:00:00.000', 5000,2, 'slide_2.jpg', 'NEW-HP_26.jpg', 'mobile_06.jpg', '1 BTC');
insert into Lottery values (6, '2018-08-06 00:00:00.000', 5000,2, 'slide_2.jpg', 'NEW-HP_26.jpg', 'mobile_06.jpg', '1 BTC');
insert into Lottery values (7, '2018-08-06 00:00:00.000', 5000,2, 'slide_2.jpg', 'NEW-HP_26.jpg', 'mobile_06.jpg', '1 BTC');
insert into Lottery values (8, '2018-08-06 00:00:00.000', 5000,2, 'slide_2.jpg', 'NEW-HP_26.jpg', 'mobile_06.jpg', '1 BTC');

insert into LotteryPrize values ('1st',	100000,	(select Id from Lottery where Phase = 5),	1,	'bg-warning');
insert into LotteryPrize values ('2nd',	20000,	(select Id from Lottery where Phase = 5),	5,	'bg-primary');
insert into LotteryPrize values ('3rd',	1000,	(select Id from Lottery where Phase = 5),	25,	'bg-danger');
insert into LotteryPrize values ('4th',	500,	(select Id from Lottery where Phase = 5),	500, 'bg-success');

insert into LotteryPrize values ('1st',	100000,	(select Id from Lottery where Phase = 6),	1,	'bg-warning');
insert into LotteryPrize values ('2nd',	20000,	(select Id from Lottery where Phase = 6),	5,	'bg-primary');
insert into LotteryPrize values ('3rd',	1000,	(select Id from Lottery where Phase = 6),	25,	'bg-danger');
insert into LotteryPrize values ('4th',	500,	(select Id from Lottery where Phase = 6),	500, 'bg-success');

insert into LotteryPrize values ('1st',	100000,	(select Id from Lottery where Phase = 7),	1,	'bg-warning');
insert into LotteryPrize values ('2nd',	20000,	(select Id from Lottery where Phase = 7),	5,	'bg-primary');
insert into LotteryPrize values ('3rd',	1000,	(select Id from Lottery where Phase = 7),	25,	'bg-danger');
insert into LotteryPrize values ('4th',	500,	(select Id from Lottery where Phase = 7),	500, 'bg-success');

insert into LotteryPrize values ('1st',	100000,	(select Id from Lottery where Phase = 8),	1,	'bg-warning');
insert into LotteryPrize values ('2nd',	20000,	(select Id from Lottery where Phase = 8),	5,	'bg-primary');
insert into LotteryPrize values ('3rd',	1000,	(select Id from Lottery where Phase = 8),	25,	'bg-danger');
insert into LotteryPrize values ('4th',	500,	(select Id from Lottery where Phase = 8),	500, 'bg-success');

insert into LangDetail values (1, N'TicketsLeft', N'tickets left');
insert into LangDetail values (2, N'TicketsLeft', N'左のチケット');

insert into LangDetail values (1, N'Buy', N'Buy');
insert into LangDetail values (2, N'Buy', N'購入');