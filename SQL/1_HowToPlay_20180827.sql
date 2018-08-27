Use CPL;

-- LangDetail
INSERT INTO LangDetail Values (1, N'HowToRegister', N'How To Register');
INSERT INTO LangDetail Values (2, N'HowToRegister', N'登録方法');

INSERT INTO LangDetail Values (1, N'HowToDepositAndExchange', N'Deposit & Exchange');
INSERT INTO LangDetail Values (2, N'HowToDepositAndExchange', N'入金＆CPL交換方法');

INSERT INTO LangDetail Values (1, N'HowToPlayLottery', N'How to play');
INSERT INTO LangDetail Values (2, N'HowToPlayLottery', N'遊び方');

INSERT INTO LangDetail Values (1, N'HowToWithDraw', N'Withdraw');
INSERT INTO LangDetail Values (2, N'HowToWithDraw', N'出金方法');

INSERT INTO LangDetail Values (1, N'HowToPlayDesktopVersion', N'Desktop Version');
INSERT INTO LangDetail Values (2, N'HowToPlayDesktopVersion', N'デスクトップ版');

INSERT INTO LangDetail Values (1, N'HowToPlayMobileVersion', N'Mobile Version');
INSERT INTO LangDetail Values (2, N'HowToPlayMobileVersion', N'モバイル版');

-- LangMsgDetail
-- Desktop
INSERT INTO LangMsgDetail Values (1, N'HowToRegisterDesktop', N'<p>CRYPTOLOT is a game site that anyone can participate using a cryptocurrency.</p>
                                        <p>We are currently opening a crypto lottery game. You can withdraw cryptocurrency if you win the lottery game.</p>
                                        <p>CRYPTOLOT can be enjoyed in the following way.</p>
                                        <br />
                                        <p><span class="font-weight-bold">1-1.</span> CRYPTOLOT''s User registration is simple.</p>
                                        <p class="ml-2">- Click "Register" on the top page.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/1-1.png" />

                                        <p><span class="font-weight-bold">1-2.</span> The member registration screen will be displayed,</p>
                                        <p class="ml-2">- Please enter your email address and password.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/1-2.png" />

                                        <p><span class="font-weight-bold">1-3.</span> A confirmation email will be sent to your email address.</p>
                                        <p class="ml-2">- Please click the link included.</p>
                                        <p><span class="font-weight-bold">1-4．</span>Registration is completed. Please login from the top page.</p>');
INSERT INTO LangMsgDetail Values (2, N'HowToRegisterDesktop', N'<p>CRYPTOLOTは、仮想通貨をお持ちの方ならだれでも手軽に遊ぶことのできるゲームサイトです。 </p>
                                        <p>現在、仮想通貨くじをオープンしています。くじで当選した賞金は、ビットコイン、イーサリアムで引き出すことが可能です。 </p>
                                        <p>CRYPTOLOTは以下の方法で楽しむことができます。 </p>
                                        <br />
                                        <p><span class="font-weight-bold">1-1.</span> CRYPTOLOTの会員登録は簡単です。 </p>
                                        <p class="ml-2">- トップページの「登録」をクリックします。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/1-1.png" />

                                        <p><span class="font-weight-bold">1-2.</span> 会員登録画面が表示されますので、お客様のメールアドレス、 </p>
                                        <p class="ml-2">- ご希望のパスワードを入力してください。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/1-2.png" />

                                        <p><span class="font-weight-bold">1-3.</span> フォームに入力されたメールアドレスに登録確認用のメールが届きます。 </p>
                                        <p class="ml-2">- その中のリンクをクリックしてください。 </p>
                                        <p><span class="font-weight-bold">1-4．</span>登録が完了いたしました。トップページのログインフォームからログインしてください。 </p>');

INSERT INTO LangMsgDetail Values (1, N'HowToDepositAndExchangeDesktop', N'<p><span class="font-weight-bold">2-1.</span> Deposit</p>
                                        <p>CRYPROLOT can be deposited by BTC, ETH.</p>
                                        <p>Enter the BTC and ETH you want to deposit in to amount in the BTC or ETH amount field, please deposit to the displayed address.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/2-1.png" />

                                        <p><span class="font-weight-bold">2-2.</span> Exchange.</p>
                                        <p>The user can participate in the game by exchanging BTC or ETH to CPL.</p>
                                        <p>The user can exchange BTC or ETH to CPL in exchange page.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/2-2.png" />

                                        <p>In the "BTC amount", please input the amount of BTC you want to exchange. In the same way, in the "ETH amount", please input the amount of ETH you wish to exchange and click the "next" button.</p>
                                        <p>Exchange from CPL to BTC, ETH is also possible at any time.</p>');
INSERT INTO LangMsgDetail Values (2, N'HowToDepositAndExchangeDesktop', N'<p><span class="font-weight-bold">2-1.</span> 入金 </p>
                                        <p>CRYPROLOTは、BTC、ETHによる入金が可能です。 </p>
                                        <p>
                                            ログイン後、メニューの入金/出金の中から入金を選択し、ご希望のBTC、ETH入金金額を入力後、
                                            表示されているアドレスに入金してください。
                                        </p>
                                        <p>（下のキャプチャーはモザイクがかかっていますが、実際にはアドレスが表示されます。） </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/2-1.png" />

                                        <p><span class="font-weight-bold">2-2.</span> 交換.</p>
                                        <p>CRYPTOLOTは、入金頂いたBTC、ETHをCPLに交換することによってゲームに参加することができます。 </p>
                                        <p>BTC,ETHからCPLへの交換は、ログイン後のメニューの「交換」をクリックして行います。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/2-2.png" />

                                        <p>「BTCの数量」の部分には交換を希望するBTC量、「ETHの数量」の部分には交換を希望するETH量を入力して、「次」ボタンをクリックしてください。 </p>
                                        <p>なお、CPLからBTC、ETHへの交換も、上のキャプチャーの矢印ボタンをクリックすることによって可能です。 </p>');

INSERT INTO LangMsgDetail Values (1, N'HowToPlayLotteryDesktop', N'<p>If you can exchange for CPL, let''s purchase lottery.</p>
                                        <p class="font-weight-bold">* The above screen is a screen under development version, some specifications may have changed.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/3(1).png" />

                                        <p>When user enters the number of tickets to purchase, the necessary CPL amount is calculated.</p>
                                        <p>Let''s click and check the contents of the lottery game to be purchased.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/3(2).png" />

                                        <p>When user enters the number of tickets to purchase, the necessary CPL amount is calculated.</p>
                                        <p>Let''s click and check the contents of the lottery game to be purchased.</p>
                                        <p>Purchase is confirmed.</p>
                                        <p>We make a lottery on the ETH Smart Contract from midnight on the day when the maximum number was reached the sales maximum number,</p>
                                        <p>The results will be announced at 10 o''clock the next morning.</p>
                                        <br />
                                        <p>You can check the results on the menu.</p>
                                        <p>You can check the result on the "History" -> "Game" page of the menu.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/3(3).png" />');
INSERT INTO LangMsgDetail Values (2, N'HowToPlayLotteryDesktop', N'<p>CPLに交換できたら、仮想通貨くじを購入してみましょう！ </p>
                                        <p class="font-weight-bold">※ 画面は開発中の画面のため、一部仕様が変更になっている場合があります。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/3(1).png" />

                                        <p>購入する枚数を入力すると、購入に必要なCPL量が計算されます。 </p>
                                        <p>クリックして、購入するくじの内容を確認しましょう。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/3(2).png" />

                                        <p>購入が確定されました。 </p>
                                        <p>仮想通貨くじは、１つのくじごとに販売上限枚数に達すると、</p>
                                        <p>上限枚数に達した日の午前０時からETHスマートコントラクト上で抽選を行い、 </p>
                                        <p>翌朝の10時に結果が発表されます。 </p>
                                        <br />
                                        <p>結果は、メニューの「履歴」->「ゲーム」ページの下部分でご確認いただけます。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/3(3).png" />');

INSERT INTO LangMsgDetail Values (1, N'HowToWithDrawDesktop', N'<p>You need to submit your identification (KYC) to withdraw.</p>
                                        <p>After KYC approved by the administration, you can withdraw BTC or ETH.</p>
                                        <br />
                                        <p><span class="font-weight-bold">4-1.</span> How to submit identification</p>
                                        <p>CRYPTOLOT accepts KYC by using passport or driver''s license.</p>
                                        <p>Please confirm from the following screen to submit.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/4-1(1).png" />

                                        <p>Please submit it from the following screen.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/4-1(2).png" />

                                        <p>Please allow 1-2 business days to confirm your identity.</p>
                                        <p>After KYC approval, BTC, ETH can be withdrawn from the payment / withdrawal menu of the dashboard.</p>

                                        <br />
                                        <p><span class="font-weight-bold">4-2.</span> How to withdraw</p>
                                        <p>Please input that you desire withdrawal amount in field of "amount" and input in your BTC or ETH address in the field of "address", and then click the withdraw button.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-en/4-2.png" />

                                        <p>Regarding the BTC or ETH address, please input correct address. If you send it to the wrong address, please notice that the asset may be lost.</p>');
INSERT INTO LangMsgDetail Values (2, N'HowToWithDrawDesktop', N'<p>出金には、お客様の身分証明書の提出（KYC）が必要です。 </p>
                                        <p>身分証明書の提出は以下の通りです。 </p>
                                        <br />
                                        <p><span class="font-weight-bold">4-1.</span> 身分証明書のご提出方法 </p>
                                        <p>CRYPTOLOTでは、パスポートまたは運転免許証によるお客様確認を受け付けています。 </p>
                                        <p>ユーザー名をクリックするとプルダウンメニューに「KYC」の項目が表示されます。このリンクをクリックすると、KYCのページが表示されます。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/4-1(1).png" />

                                        <p>ご提出は、以下の画面からご確認ください。 </p>
                                        <p>（上記のような表示にならない場合には、ユーザープロフィールを入力していない可能性があります。ユーザープロフィールを入力後、再度このページ訪れると正しく表示されます。） </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/4-1(2).png" />

                                        <p>身分証明書の確認には1-2日営業日を頂きます。 </p>
                                        <p>KYC承認後、ダッシュボードの入金/出金メニューからお客様のBTC、ETHを出金することができます。 </p>

                                        <br />
                                        <p><span class="font-weight-bold">4-2.</span> 出金方法 </p>
                                        <p>

                                            ご希望の出金量を「量」の項目に、お客様のBTCまたはETHアドレスをいずれかの「アドレス」欄に入力して、出金ボタンをクリックしてください。
                                        </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/desktop-jp/4-2.png" />

                                        <p>上記のアドレスは、必ず正しいアドレスを入力してください。間違ったアドレスに送ると、資産が失われる場合がありますので、ご注意ください。 </p>');

-- Mobile
INSERT INTO LangMsgDetail Values (1, N'HowToRegisterMobile', N'<p>CRYPTOLOT is a game site that anyone can participate using a cryptocurrency.</p>
                                        <p>We are currently opening a crypto lottery game. You can withdraw cryptocurrency if you win the lottery game.</p>
                                        <p>CRYPTOLOT can be enjoyed in the following way.</p>
                                        <br />
                                        <p><span class="font-weight-bold">1-1.</span> CRYPTOLOT''s User registration is easy.</p>
                                        <p class="ml-2">- Click "Register" on the top page.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-en/1-1.png" />

                                        <p><span class="font-weight-bold">1-2.</span> The member registration screen will be displayed,</p>
                                        <p class="ml-2">- Please enter your email address and password.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-en/1-2.png" />

                                        <p><span class="font-weight-bold">1-3.</span> A confirmation email will be sent to your email address.</p>
                                        <p class="ml-2">- Please click the link included.</p>
                                        <p><span class="font-weight-bold">1-4．</span>Registration is completed. Please login from the top page.</p>');
INSERT INTO LangMsgDetail Values (2, N'HowToRegisterMobile', N'<p><span class="font-weight-bold">1-1.</span> CRYPTOLOTの会員登録は簡単です。</p>
                                        <p class="ml-2">- トップページの「登録」をタップします。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/1-1.png" />

                                        <p><span class="font-weight-bold">1-2.</span> 会員登録画面が表示されますので、お客様のメールアドレス、ご希望のパスワードを入力してください。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/1-2.png" />

                                        <p><span class="font-weight-bold">1-3.</span>  フォームに入力されたメールアドレスに登録確認用のメールが届きます。 </p>
                                        <p class="ml-2">- その中のリンクをタップしてください。 </p>
                                        <p><span class="font-weight-bold">1-4．</span>登録完了画面が表示されると、登録は完了です。トップページのログインフォームからログインしてください。 </p>');

INSERT INTO LangMsgDetail Values (1, N'HowToDepositAndExchangeMobile', N'<p><span class="font-weight-bold">2-1.</span> Deposit</p>
                                        <p>CRYPROLOT can be deposited by BTC, ETH.</p>
                                        <p>Enter the BTC and ETH you want to deposit in to amount in the BTC or ETH amount field, please deposit to the displayed address.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-en/2-1(1).png" />
                                        <br />
                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-en/2-1(2).png" />

                                        <p><span class="font-weight-bold">2-2.</span> Exchange.</p>
                                        <p>The user can participate in the game by exchanging BTC or ETH to CPL.</p>
                                        <p>The user can exchange BTC or ETH to CPL in exchange page.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-en/2-2(1).png" />
                                        <br />
                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-en/2-2(2).png" />

                                        <p>In the "BTC amount", please input the amount of BTC you want to exchange. In the same way, in the "ETH amount", please input the amount of ETH you wish to exchange and click the "next" button.</p>
                                        <p>Exchange from CPL to BTC, ETH is also possible at any time.</p>');
INSERT INTO LangMsgDetail Values (2, N'HowToDepositAndExchangeMobile', N'<p><span class="font-weight-bold">2-1.</span> 入金 </p>
                                        <p>CRYPROLOTは、BTC、ETHによる入金が可能です。 </p>
                                        <p>ログイン後、メニューの入金/出金の中から入金を選択し、ご希望のBTCまたはETH入金金額を入力後、表示されているアドレスに入金してください。 </p>
                                        <p>（以下のキャプチャーはモザイクがかかっていますが、実際にはアドレスが表示されます。） </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/2-1(1).png" />
                                        <br />
                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/2-1(2).png" />

                                        <p><span class="font-weight-bold">2-2.</span> 交換.</p>
                                        <p>CRYPTOLOTでは、入金頂いたBTC、ETHをCPLに交換することによってゲームに参加することができます。 </p>
                                        <p>BTC,ETHからCPLへの交換は、ログイン後のメニューの「交換」をタップして行います。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/2-2(1).png" />
                                        <br />
                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/2-2(2).png" />

                                        <p>「BTCの数量」の部分には交換を希望するBTC量、「ETHの数量」の部分には交換を希望するETH量を入力して、「次」ボタンをタップしてください。 </p>
                                        <p>なお、CPLからBTC、ETHへの交換も、上のキャプチャーの矢印ボタンをタップすることによって可能です。 </p>');

INSERT INTO LangMsgDetail Values (1, N'HowToPlayLotteryMobile', N'<p>If you can exchange for CPL, let''s purchase lottery.</p>
                                        <p class="font-weight-bold">* The above screen is a screen under development version, some specifications may have changed.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-en/3-1.png" />

                                        <p>When user enters the number of tickets to purchase, the necessary CPL amount is calculated.</p>
                                        <p>Let''s click and check the contents of the lottery game to be purchased.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-en/3-2.png" />

                                        <p>When user enters the number of tickets to purchase, the necessary CPL amount is calculated.</p>
                                        <p>Let''s click and check the contents of the lottery game to be purchased.</p>
                                        <p>Purchase is confirmed.</p>
                                        <p>We make a lottery on the ETH Smart Contract from midnight on the day when the maximum number was reached the sales maximum number,</p>
                                        <p>The results will be announced at 10 o''clock the next morning.</p>
                                        <br />
                                        <p>You can check the results on the menu.</p>
                                        <p>You can check the result on the "History" -> "Game" page of the menu.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-en/3-3.png" />');
INSERT INTO LangMsgDetail Values (2, N'HowToPlayLotteryMobile', N'<p>CPLに交換できたら、仮想通貨くじを購入してみましょう！ </p>
                                        <p>トップページから、購入したいくじのバナーをタップします。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/3(1).png" />

                                        <p class="font-weight-bold">※画面は開発中の画面のため、一部仕様が変更になっている場合があります。ご注意ください。 </p>
                                        <br />
                                        <p>購入画面移動後、フォームより購入したい枚数を入力すると、購入に必要なCPL量が計算されます。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/3(2).png" />

                                        <p>入力後、「購入」をタップして購入するくじの内容を確認しましょう。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/3(3).png" />

                                        <p>
                                            購入が確定されました。
                                        </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/3(4).png" />

                                        <p>仮想通貨くじは、１つのくじごとに販売上限枚数に達すると、上限枚数に達した日の午前０時からETHスマートコントラクト上で抽選を行い、翌朝の10時に結果が発表されます。 </p>
                                        <p>結果は、メニューの「履歴」->「ゲーム」ページでご確認いただけます。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/3(5).png" />
                                        <br />
                                        <img class="img-fluid img-thumbnail border-0 my-1" src="/images/howtoplay/mobile-jp/3(6).png" />');

INSERT INTO LangMsgDetail Values (1, N'HowToWithDrawMobile', N'<p>You need to submit your identification (KYC) to withdraw.</p>
                                        <p>After KYC approved by the administration, you can withdraw BTC or ETH.</p>
                                        <br />
                                        <p><span class="font-weight-bold">4-1.</span> How to submit identification</p>
                                        <p>CRYPTOLOT accepts KYC by using passport or driver''s license.</p>
                                        <p>Please confirm from the following screen to submit.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-en/4-1(1).png" />

                                        <p>Please submit it from the following screen.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-en/4-1(2).png" />

                                        <p>Please allow 1-2 business days to confirm your identity.</p>
                                        <p>After KYC approval, BTC, ETH can be withdrawn from the payment / withdrawal menu of the dashboard.</p>

                                        <br />
                                        <p><span class="font-weight-bold">4-2.</span> How to withdraw</p>
                                        <p>Please input that you desire withdrawal amount in field of "amount" and input in your BTC or ETH address in the field of "address", and then click the withdraw button.</p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-en/4-2.png" />

                                        <p>Regarding the BTC or ETH address, please input correct address. If you send it to the wrong address, please notice that the asset may be lost.</p>');
INSERT INTO LangMsgDetail Values (2, N'HowToWithDrawMobile', N'<p>出金には、お客様の身分証明書の提出（KYC）が必要です。 </p>
                                        <p>身分証明書の提出は以下の通りです。 </p>
                                        <br />
                                        <p><span class="font-weight-bold">4-1.</span> 身分証明書のご提出方法 </p>
                                        <p>CRYPTOLOTでは、パスポートまたは運転免許証によるお客様確認を受け付けています。 </p>
                                        <p>ユーザー名をタップするとプルダウンメニューに「KYC」の項目が表示されますので、タップするとKYCのページにリンクされます。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-jp/4-1(1).png" />

                                        <p>ご提出は、以下の画面で行ってください。身分証明書の確認には2-3営業日を頂きます。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-jp/4-1(2).png" />

                                        <p>運営事務局によるKYC承認後、ダッシュボードの入金/出金メニューからお客様のBTC、ETHを出金することができます。 </p>

                                        <br />
                                        <p><span class="font-weight-bold">4-2.</span> 出金方法 </p>
                                        <p>ご希望の出金量を「量」の項目に、お客様のBTCまたはETHアドレスをいずれかの「アドレス」欄に入力して、入金/出金ボタンをタップしてください。 </p>

                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-jp/4-2(1).png" />
                                        <br />
                                        <img class="img-fluid img-thumbnail border-0 my-1"src="/images/howtoplay/mobile-jp/4-2(2).png" />

                                        <p>アドレス欄に入力するアドレスは、必ず正しいアドレスを入力してください。間違ったアドレスに送ると、資産が失われる場合がありますのでご注意ください。 </p>
                                        <p>上記の内容についてご質問がある場合には、コンタクトフォームよりお問い合わせください。 </p>');