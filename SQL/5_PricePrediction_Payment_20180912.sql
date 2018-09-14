use CPL;

--Update setting for price prediction
update Setting set Name = 'PricePredictionHoldingIntervalInHour' where Name = 'HoldingIntervalInHour';
update Setting set Name = 'PricePredictionCompareIntervalInMinute' where Name = 'CompareIntervalInMinute';
update Setting set Name = 'PricePredictionDailyStartTimeInHour' where Name = 'DailyStartTimeInHour';
update Setting set Name = 'PricePredictionDailyStartTimeInMinute' where Name = 'DailyStartTimeInMinute';

--Add new columns for Affiliate table
ALTER TABLE Affiliate ADD IsAutoPaymentEnable bit not null default(0);
ALTER TABLE Affiliate ADD IsTier2TabVisible bit not null default(0);
ALTER TABLE Affiliate ADD IsTier3TabVisible bit not null default(0);

insert into Setting values (N'PaymentCreateMonthlyStartTimeInDay', 1, null);
insert into Setting values (N'PaymentCreateMonthlyStartTimeInHour', 0, null);
insert into Setting values (N'PaymentCreateMonthlyStartTimeInMinute', 0, null);

insert into Setting values (N'PaymentProcessMonthlyStartTimeInDay', 10, null);
insert into Setting values (N'PaymentProcessMonthlyStartTimeInHour', 0, null);
insert into Setting values (N'PaymentProcessMonthlyStartTimeInMinute', 0, null);