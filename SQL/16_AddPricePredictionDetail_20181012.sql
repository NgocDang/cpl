UPDATE PricePrediction SET PricePredictionCategoryId = 1


truncate table PricePredictionDetail
insert into PricePredictionDetail
SELECT CASE DATEPART(hh,OpenBettingTime)  
	   WHEN 1 THEN '24:00'
	   WHEN 9 THEN '8:00'
	   WHEN 17 THEN '16:00' 
	   END AS TITLE,
	   N'Just choose High or Low! <br/>
There are three games a day!',
	   1,
	   Id
FROM PricePrediction;

GO

insert into PricePredictionDetail
SELECT CASE DATEPART(hh,OpenBettingTime)  
	   WHEN 1 THEN '24:00'
	   WHEN 9 THEN '8:00'
	   WHEN 17 THEN '16:00' 
	   END AS TITLE,
	   N'HighかLowを選ぶだけ！<br/>
1日３回のチャンスを掴むのはあなたです！',
	   2,
	   Id
FROM PricePrediction;


Update LotteryDetail
set ShortDescription = '0.0001 BTC<br/>
100 BTC highest prize!<br/>
An opportunity to be millionaire'
where LangId = 1;


Update LotteryDetail
set ShortDescription = '0.0001BTC​<br/>
で遊べる宝くじ！​<br/>
最高当選１００BTC！​<br/>
億万長者のチャンス！​'
where LangId = 2;


