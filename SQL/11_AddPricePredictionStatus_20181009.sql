USE [CPL];

ALTER TABLE PricePrediction 
	ADD Status INT NOT NULL DEFAULT 1;

insert into LangDetail values (1, N'WaitToNextGame', N'Please wait for next game');
insert into LangDetail values (2, N'WaitToNextGame', N'次の試合を待ってください');