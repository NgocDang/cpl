﻿--- LAN - Home rebuild - 2018/09/28
use CPL;

insert into LangDetail values (1, N'PartnerCompanies', N'Partner games companies');
insert into LangDetail values (2, N'PartnerCompanies', N'提携ゲーム会社');

insert into LangDetail values (1, N'License', N'License');
insert into LangDetail values (2, N'License', N'ライセンス');

insert into LangDetail values (1, N'WhatIsBitcoin', N'What is Bitcoin?');
insert into LangDetail values (2, N'WhatIsBitcoin', N'ビットコインとは');

insert into LangDetail values (1, N'PersonalInformationProtection', N'Personal information protection');
insert into LangDetail values (2, N'PersonalInformationProtection', N'個人情報保護について');

update LangDetail set Name = N'AboutCPO', Value = N'About CPO' where Name = N'AboutCPL' and LangId = 1;
update LangDetail set Name = N'AboutCPO', Value = N'CPOについて' where Name = N'AboutCPL' and LangId = 2;

delete LangDetail where Name = 'WhatIsCPL';
delete LangMsgDetail where Name = 'WhatIsCPL';

insert into LangDetail values (1, N'WhatIsCryptoLottery', N'What is Crypto Lottery?');
insert into LangDetail values (2, N'WhatIsCryptoLottery', N'暗号宝くじとは?');

insert into LangDetail values (1, N'WhatIsCryptoPricePrediction', N'What is Crypto Price Prediction?');
insert into LangDetail values (2, N'WhatIsCryptoPricePrediction', N'暗号の価格予測とは?');

insert into LangDetail values (1, N'WhatIsCryptoCasino', N'What is Crypto Casino?');
insert into LangDetail values (2, N'WhatIsCryptoCasino', N'クリプトカジノとは?');

insert into LangDetail values (1, N'Campaign', N'Campaign');
insert into LangDetail values (2, N'Campaign', N'キャンペーン');

insert into LangDetail values (1, N'PhoneNumber', N'Phone number');
insert into LangDetail values (2, N'PhoneNumber', N'電話番号');

insert into LangDetail values (1, N'PhoneNumberRequired', N'Please fill in phone number');
insert into LangDetail values (2, N'PhoneNumberRequired', N'電話番号を記入してください');

insert into LangDetail values (1, N'Message', N'Message');
insert into LangDetail values (2, N'Message', N'メッセージ');

insert into LangDetail values (1, N'MessageRequired', N'Please fill in message');
insert into LangDetail values (2, N'MessageRequired', N'メッセージを記入してください');

--- DamTran - What is Crypto Odds - 2018/10/01
USE CPL;

INSERT INTO LangDetail VALUES (1, N'WhatIsCryptoOdds', N'What is Crypto Odds');
INSERT INTO LangDetail VALUES (2, N'WhatIsCryptoOdds', N'クリプトオッズとは');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoOdds', 
N'Crypto Odds is an online gaming site holding online gaming licenses issued by the government of the Netherlands Curacao. <br />
Crypto Odds is an on-line casino that maintains anonymity by depositing and withdrawing in virtual currency and features fast deposits and withdrawals. Please enjoy from one registration to get one rushrack with one smartphone. <br />
We use a system that randomly generates numbers called smart contract technology and random number generator (RNG) for contents, and we provide safe and secure gaming with a system that eliminates fraud.');
INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoOdds',
N'クリプトオッズはオランダ領キュラソー政府発行のオンラインゲーミングライセンスを保有するオンラインゲーミングサイトです。<br />
クリプトオッズは仮想通貨での入出金により匿名性を保ち、迅速な入出金が特徴のオンラインカジノです。スマホ一つで登録から一攫千金までお楽しみください。<br />
コンテンツにはスマートコントラクト技術やランダムナンバージェネレーター(RNG)と呼ばれる数字をランダムに生成するシステムを使用しており、不正を排除したシステムで安全・安心なゲーミングを提供しています。');

-- DamTran - Easy 3 steps - 2018/10/01
USE CPL
INSERT INTO LangDetail VALUES (1, N'ForEnjoyingCryptOdds', N'For enjoying Crypto Odds');
INSERT INTO LangDetail VALUES (2, N'ForEnjoyingCryptOdds', N'クリプトオッズを楽しむ為の');

INSERT INTO LangDetail VALUES (1, N'Easy3Steps', N'3 Simple Steps');
INSERT INTO LangDetail VALUES (2, N'Easy3Steps', N'簡単・楽々　３ステップ');

INSERT INTO LangDetail VALUES (1, N'EasyRegistration', N'Easy Registration');
INSERT INTO LangDetail VALUES (2, N'EasyRegistration', N'楽々登録');

INSERT INTO LangDetail VALUES (1, N'EasyRegistrationDetail',
 N'Personal identity is absolutely unnecessary! <br />
Easy registration with e-mail and password!');
INSERT INTO LangDetail VALUES (2, N'EasyRegistrationDetail', 
N'身分証登録一切不要！ <br />
メールアドレスとパスワード設定のみで簡単登録！');
INSERT INTO LangDetail VALUES (1, N'RegisterFor30Seconds', N'30-seconds registration only');
INSERT INTO LangDetail VALUES (2, N'RegisterFor30Seconds', N'３０秒登録');

INSERT INTO LangDetail VALUES (1, N'BTCDeposit', N'BTC Deposit');
INSERT INTO LangDetail VALUES (2, N'BTCDeposit', N'BTC入金');

INSERT INTO LangDetail VALUES (1, N'BTCDepositDetail',
 N'Just deposit BTC to the designated address! <br />
 BTC is automatically converted to CPO.');
INSERT INTO LangDetail VALUES (2, N'BTCDepositDetail', 
N'指定のアドレスにBTCを送金するだけ！<br />
自動BTCがCPOに変換されます。');

INSERT INTO LangDetail VALUES (1, N'PlayGameDetail',
 N'Please choose your favorite game <br />Please enjoy the Crypto Odds as much as you want');
INSERT INTO LangDetail VALUES (2, N'PlayGameDetail', 
N'お好みのゲームを選択して <br />思う存分クリプトオッズをお楽しみ下さい');


--- Slider --
INSERT INTO LangDetail VALUES (1, N'Url', N'Url');
INSERT INTO LangDetail VALUES (2, N'Url', N'Url');

INSERT INTO LangDetail VALUES (1, N'NameRequired', N'Please fill in name!');
INSERT INTO LangDetail VALUES (2, N'NameRequired', N'名前を記入してください!');

INSERT INTO LangDetail VALUES (1, N'DesktopImageRequired', N'Please fill in image for desktop!');
INSERT INTO LangDetail VALUES (2, N'DesktopImageRequired', N'デスクトップ用の画像を入力してください！');

INSERT INTO LangDetail VALUES (1, N'MobileImageRequired', N'Please fill in image for mobile!');
INSERT INTO LangDetail VALUES (2, N'MobileImageRequired', N'モバイル用の画像を入力してください！');

INSERT INTO LangDetail VALUES (1, N'DesktopImage', N'Desktop image');
INSERT INTO LangDetail VALUES (2, N'DesktopImage', N'デスクトップイメージ');

INSERT INTO LangDetail VALUES (1, N'MobileImage', N'Mobile image');
INSERT INTO LangDetail VALUES (2, N'MobileImage', N'モバイルイメージ');

INSERT INTO LangDetail VALUES (1, N'Group', N'Group');
INSERT INTO LangDetail VALUES (2, N'Group', N'グループ');

INSERT INTO LangDetail VALUES (1, N'BeTheFirstPerson', N'Be the first');
INSERT INTO LangDetail VALUES (2, N'BeTheFirstPerson', N'はじめての方はこちら');
-- DamTran - PricePredictonStatisticsChart
USE CPL;
INSERT INTO LangDetail VALUES (1, N'NumberOfPrediction', N'Number Of Prediction');
INSERT INTO LangDetail VALUES (2, N'NumberOfPrediction', N'予測数');

INSERT INTO LangDetail VALUES (1, N'PricePredictionCategory', N'Price Prediction Category');
INSERT INTO LangDetail VALUES (2, N'PricePredictionCategory', N'価格予測カテゴリ');

INSERT INTO LangDetail VALUES (1, N'AddNewCategory', N'Add new category');
INSERT INTO LangDetail VALUES (2, N'AddNewCategory', N'新しいカテゴリを追加');

INSERT INTO LangDetail VALUES (1, N'PricePredictionCategoryNameRequired', N'Please fill in price prediction category name');
INSERT INTO LangDetail VALUES (2, N'PricePredictionCategoryNameRequired', N'価格予測カテゴリ名を記入してください');

-- DamTran - PricePredictonSetting - 20181004
INSERT INTO LangDetail VALUES (1, N'PricePredictionCategoryRequired', N'Please select price prediction category');
INSERT INTO LangDetail VALUES (2, N'PricePredictionCategoryRequired', N'価格予測カテゴリを選択してください');

--- Price Prediction Setting
INSERT INTO LangDetail VALUES (1, N'PricePredictionSetting', N'Price Prediction Setting');
INSERT INTO LangDetail VALUES (2, N'PricePredictionSetting', N'価格予測設定');

INSERT INTO LangDetail VALUES (1, N'BettingTime', N'Betting Time');
INSERT INTO LangDetail VALUES (2, N'BettingTime', N'ベットタイム');

INSERT INTO LangDetail VALUES (1, N'HoldingTimeInterval', N'Holding Time Interval');
INSERT INTO LangDetail VALUES (2, N'HoldingTimeInterval', N'待ち時間');

INSERT INTO LangDetail VALUES (1, N'ResultTimeInterval', N'Result Time Interval');
INSERT INTO LangDetail VALUES (2, N'ResultTimeInterval', N'ラッフルの時間');

INSERT INTO LangDetail VALUES (1, N'DividendRate', N'Dividend Rate');
INSERT INTO LangDetail VALUES (2, N'DividendRate', N'配当率');

-- DamTran - AddPricePredictonSetting - 20181004
INSERT INTO LangDetail VALUES (1, N'HoldingTimeIntervalRequired', N'Please fill in holding time interval!');
INSERT INTO LangDetail VALUES (2, N'HoldingTimeIntervalRequired', N'保有期間を記入してください!');

INSERT INTO LangDetail VALUES (1, N'ResultTimeIntervalRequired', N'Please fill in result time interval!');
INSERT INTO LangDetail VALUES (2, N'ResultTimeIntervalRequired', N'結果の時間間隔を記入してください!');

INSERT INTO LangDetail VALUES (1, N'DividendRateRequired', N'Please fill in dividend rate!');
INSERT INTO LangDetail VALUES (2, N'DividendRateRequired', N'必要な配当率を記入してください!');

INSERT INTO LangDetail VALUES (1, N'CloseBettingTime', N'Close betting time');
INSERT INTO LangDetail VALUES (2, N'CloseBettingTime', N'賭け時間を閉じる');

INSERT INTO LangDetail VALUES (1, N'OpenBettingTime', N'Open betting time');
INSERT INTO LangDetail VALUES (2, N'OpenBettingTime', N'オープンベッティング時間');

-- What is crypto lottery----
INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryMainContent', N'<p>仮想通貨宝クジとは、<span class="text-warning">ブロックチェーン</span>とスマートコ<span class="text-warning">ントラク</span>ト技術が活用された宝くじプラットホームです。</p>
                                    <p>ブロックチェーン技術により全てのクジの履歴をトランザクションで確認する事が出来るようにし、透明性の高い宝くじを実現されております。</p>
                                    <p class="text-muted font-size-13px">＊お客様の名前などの個人情報は一切公開されませんのでご安心ください。</p>
                                    <br />
                                    <p>また、スマートコントラクトにより、クジの発行や管理に一切人間の手が加える事ができないので、不正や外的要因（ハッキングなど）からシステムを守り、公正で安全な宝くじゲームをユーザーの皆様に提供いたしております。</p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryMainContent', N'<p>仮想通貨宝クジとは、<span class="text-warning">ブロックチェーン</span>とスマートコ<span class="text-warning">ントラク</span>ト技術が活用された宝くじプラットホームです。</p>
                                    <p>ブロックチェーン技術により全てのクジの履歴をトランザクションで確認する事が出来るようにし、透明性の高い宝くじを実現されております。</p>
                                    <p class="text-muted font-size-13px">＊お客様の名前などの個人情報は一切公開されませんのでご安心ください。</p>
                                    <br />
                                    <p>また、スマートコントラクトにより、クジの発行や管理に一切人間の手が加える事ができないので、不正や外的要因（ハッキングなど）からシステムを守り、公正で安全な宝くじゲームをユーザーの皆様に提供いたしております。</p>');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryHighestAwardTitle', N'<p class="up">
                                                最高当選
                                            </p>
                                            <p class="down">
                                                100BTC
                                            </p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryHighestAwardTitle', N'<p class="up">
                                                最高当選
                                            </p>
                                            <p class="down">
                                                100BTC
                                            </p>');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryMaximumRateTitle', N' <p class="up">
                                                最大還元率
                                            </p>
                                            <p class="down">
                                                80％
                                            </p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryMaximumRateTitle', N' <p class="up">
                                                最大還元率
                                            </p>
                                            <p class="down">
                                                80％
                                            </p>');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryLeadingClassTitle', N'<p class="up">
                                                業界トップクラスの
                                            </p>
                                            <p class="down">
                                                安全性！
                                            </p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryLeadingClassTitle', N'<p class="up">
                                                業界トップクラスの
                                            </p>
                                            <p class="down">
                                                安全性！
                                            </p>');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryHighestAwardContent', N'<p>
                                                0.0001BTCから購入ができ <br />
                                                最高当選額は過去最高の１００ <br />
                                                BTCをご用意しております。
                                            </p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryHighestAwardContent', N'<p>
                                                0.0001BTCから購入ができ <br />
                                                最高当選額は過去最高の１００ <br />
                                                BTCをご用意しております。
                                            </p>');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryMaximumRateContent', N'<p>
                                                人件費がかからないからこそ出来る <br />
                                                業界NO.1の還元率！
                                            </p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryMaximumRateContent', N'<p>
                                                人件費がかからないからこそ出来る <br />
                                                業界NO.1の還元率！
                                            </p>');

INSERT INTO LangMsgDetail VALUES (1, N'WhatIsCryptoLotteryLeadingClassContent', N'<p>
                                                ブロックチェーンとスマートコントラクト技術で、不正とハッキングからシステムを守ります！
                                            </p>');

INSERT INTO LangMsgDetail VALUES (2, N'WhatIsCryptoLotteryLeadingClassContent', N'<p>
                                                ブロックチェーンとスマートコントラクト技術で、不正とハッキングからシステムを守ります！
                                            </p>');