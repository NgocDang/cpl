Use CPL;

-- Add new column into Lottery table
ALTER TABLE Lottery ADD UnitPrice Money NOT NULL DEFAULT '0';

-- LangDetail
INSERT INTO LangDetail VALUES (1, N'NumberOfLotteryTicket', N'Number of lottery tickets');
INSERT INTO LangDetail VALUES (1, N'NumberOfLotteryTicket', N'宝くじの枚数');

INSERT INTO LangDetail VALUES (1, N'NumberOfLotteryTicketRequired', N'Please fill in number of lottery tickets!');
INSERT INTO LangDetail VALUES (1, N'NumberOfLotteryTicketRequired', N'宝くじの枚数を記入してください！');

INSERT INTO LangDetail VALUES (1, N'TicketPriceRequired', N'Please fill in ticket price!');
INSERT INTO LangDetail VALUES (1, N'TicketPriceRequired', N'チケット価格を記入してください！');

INSERT INTO LangDetail VALUES (1, N'AddAndPublish', N'Add and Publish');
INSERT INTO LangDetail VALUES (1, N'AddAndPublish', N'追加と公開');