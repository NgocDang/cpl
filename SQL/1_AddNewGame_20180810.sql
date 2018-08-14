Use CPL;

-- Add new column into Lottery table
ALTER TABLE Lottery ADD UnitPrice Money NOT NULL DEFAULT '0';

-- LangDetail
INSERT INTO LangDetail VALUES (1, N'NumberOfLotteryTicket', N'Number of lottery tickets');
INSERT INTO LangDetail VALUES (2, N'NumberOfLotteryTicket', N'宝くじの枚数');

INSERT INTO LangDetail VALUES (1, N'NumberOfLotteryTicketRequired', N'Please fill in number of lottery tickets!');
INSERT INTO LangDetail VALUES (2, N'NumberOfLotteryTicketRequired', N'宝くじの枚数を記入してください！');

INSERT INTO LangDetail VALUES (1, N'TicketPriceRequired', N'Please fill in ticket price!');
INSERT INTO LangDetail VALUES (2, N'TicketPriceRequired', N'チケット価格を記入してください！');

INSERT INTO LangDetail VALUES (1, N'AddAndPublish', N'Add and Publish');
INSERT INTO LangDetail VALUES (2, N'AddAndPublish', N'追加と公開');

INSERT INTO LangDetail VALUES (1, N'PrizeAwardRequired', N'Please fill in award of prize!');
INSERT INTO LangDetail VALUES (2, N'PrizeAwardRequired', N'賞を授与してください');

INSERT INTO LangDetail VALUES (1, N'PrizeNumberOfTicketRequired', N'Please fill in number of ticket winner of prize!');
INSERT INTO LangDetail VALUES (2, N'PrizeNumberOfTicketRequired', N'賞品のチケットの勝者数を記入してください！');

INSERT INTO LangDetail VALUES (1, N'SliderImageRequired', N'Please upload slider image for game!');
INSERT INTO LangDetail VALUES (2, N'SliderImageRequired', N'ゲームのスライダーイメージをアップロードしてください！');

INSERT INTO LangDetail VALUES (1, N'DesktopImageRequired', N'Please upload desktop version image for game!');
INSERT INTO LangDetail VALUES (2, N'DesktopImageRequired', N'ゲーム用のデスクトップバージョンイメージをアップロードしてください！');

INSERT INTO LangDetail VALUES (1, N'MobileImageRequired', N'Please upload mobile version image for game!');
INSERT INTO LangDetail VALUES (2, N'MobileImageRequired', N'ゲーム用のモバイルバージョンイメージをアップロードしてください！');

INSERT INTO LangDetail VALUES (1, N'Activate', N'Activate');
INSERT INTO LangDetail VALUES (2, N'Activate', N'活性化する');

INSERT INTO LangDetail VALUES (1, N'AddPrizeRequired', N'Please add prize for lottery game!');
INSERT INTO LangDetail VALUES (2, N'AddPrizeRequired', N'宝くじゲームの賞を追加してください！');

INSERT INTO LangDetail VALUES (1, N'SaveAndPublish', N'Save And Publish');
INSERT INTO LangDetail VALUES (2, N'SaveAndPublish', N'保存して公開する');

INSERT INTO LangDetail VALUES (1, N'ActivateSuccessfully', N'Activated Successfully');
INSERT INTO LangDetail VALUES (2, N'ActivateSuccessfully', N'有効に成功しました');

INSERT INTO LangDetail VALUES (1, N'DeleteLotteryGameConfirmation', N'Are you sure you want to delete this game?');
INSERT INTO LangDetail VALUES (2, N'DeleteLotteryGameConfirmation', N'このゲームを削除してもよろしいですか？');