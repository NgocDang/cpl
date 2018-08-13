Use CPL

INSERT INTO LangDetail VALUES(1, N'ToWalletAddress', N'To Wallet Address');
INSERT INTO LangDetail VALUES(2, N'ToWalletAddress', N'ウォレットアドレス宛');

INSERT INTO LangDetail VALUES(1, N'CoinAmount', N'Coin Amount');
INSERT INTO LangDetail VALUES(2, N'CoinAmount', N'コイン額');

INSERT INTO LangDetail VALUES(1, N'Currency', N'Currency');
INSERT INTO LangDetail VALUES(2, N'Currency', N'通貨');

INSERT INTO LangDetail VALUES(1, N'Type', N'Type');
INSERT INTO LangDetail VALUES(2, N'Type', N'通貨');

INSERT INTO LangDetail VALUES (1, N'TransactionDetail', N'Transaction Detail');
INSERT INTO LangDetail VALUES (2, N'TransactionDetail', N'取引の詳細');

INSERT INTO LangDetail VALUES (1, N'FromWalletAddress', N'From Wallet Address');
INSERT INTO LangDetail VALUES (2, N'FromWalletAddress', N'ウォレットアドレスから');

INSERT INTO LangDetail VALUES (1, N'Rate', N'Rate');
INSERT INTO LangDetail VALUES (2, N'Rate', N'レート');

--Data for testing
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGa', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshha', 500, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05da', 1, 0);
	INSERT INTO CoinTransaction VALUES(26, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGb', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhb', 100, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05db', 2, 1);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGc', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhc', 200, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dc', 3, null);
	INSERT INTO CoinTransaction VALUES(26, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGd', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhd', 300, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dd', 4, 0);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGe', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhe', 400, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05de', 5, 1);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGf', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhf', 600, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05df', 6, null);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGg', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhg', 700, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dg', 7, 0);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGh', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhh', 800, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dh', 8, 1);
	INSERT INTO CoinTransaction VALUES(26, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGi', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhi', 900, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05di', 3, null);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGj', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhj', 1000, '08-13-2018', 2, 200, 1, '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dj', 3, 0);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGk', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhk', 1100, '08-13-2018', 2, 200, 1, '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dk', 3, 1);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGl', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhl', 1200, '08-13-2018', 2, 200, 1, '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dl', 3, null);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGa', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshha', 500, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05da', 1, 0);
	INSERT INTO CoinTransaction VALUES(26, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGb', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhb', 100, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05db', 2, 1);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGc', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhc', 200, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dc', 3, null);
	INSERT INTO CoinTransaction VALUES(26, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGd', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhd', 300, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dd', 4, 0);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGe', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhe', 400, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05de', 5, 1);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGf', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhf', 600, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05df', 6, null);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGg', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhg', 700, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dg', 7, 0);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGh', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhh', 800, '08-13-2018', 1, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dh', 8, 1);
	INSERT INTO CoinTransaction VALUES(26, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGi', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhi', 900, '08-13-2018', 2, 200, 1,  '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05di', 3, null);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGj', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhj', 1000, '08-13-2018', 2, 200, 1, '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dj', 3, 0);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGk', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhk', 1100, '08-13-2018', 2, 200, 1, '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dk', 3, 1);
	INSERT INTO CoinTransaction VALUES(20, N'17A16QmavnUfCW11DAApiJxp7ARnxN5pGl', N'1Cw7peF1LHj8JaDcFVPnczfhftH8qcshhl', 1200, '08-13-2018', 2, 200, 1, '83a1e0865b1d240c784c165efd90ff50a8f6b44f74409e7383aa3747d8fd05dl', 3, null);
