Use CPL;

Insert Into LangDetail Values (1, N'GeneratedQRCodeError', N'Invalid QR Code.');
Insert Into LangDetail Values (2, N'GeneratedQRCodeError', N'無効なQRコード。');

Insert Into LangDetail Values (1, N'InvalidWithdrawAmount', N'Input amount need to be larger than 0.');
Insert Into LangDetail Values (2, N'InvalidWithdrawAmount', N'入力量は0より大きくする必要があります。');

Insert Into LangDetail Values (1, N'InvalidAmountOrInsufficientFunds', N'Invalid amount or insufficient funds.');
Insert Into LangDetail Values (2, N'InvalidAmountOrInsufficientFunds', N'無効な金額または不十分な資金。');

Insert Into LangDetail values (1, N'ContinueVerifyKYC', N'Continue verify KYC');
Insert Into LangDetail values (2, N'ContinueVerifyKYC', N'KYCの確認を続ける');

Insert Into LangDetail values (1, N'WithdrawRequireProfile', N'Profile Required!');
Insert Into LangDetail values (2, N'WithdrawRequireProfile', N'必要なプロファイル');

Insert Into LangDetail values (1, N'WithdrawRequireProfileContent', N'Your profile has not been registered. Please register before withdrawing!');
Insert Into LangDetail values (2, N'WithdrawRequireProfileContent', N'あなたのプロフィールは登録されていません。 脱退する前に登録してください！');

Insert Into LangDetail values (1, N'WithdrawRequireKYC', N'KYC Required!');
Insert Into LangDetail values (2, N'WithdrawRequireKYC', N'KYC必須！');

Insert Into LangDetail values (1, N'WithdrawRequireKYCContent', N'Your KYC has not been registered or waiting for approval. Please register and waiting for acceptance before withdrawing!');
Insert Into LangDetail values (2, N'WithdrawRequireKYCContent', N'あなたのKYCは登録されていないか、承認待ちです。 退会する前に登録し、受諾を待ってください！');

Update LangDetail set value =  N'We have received your personal KYC document, please wait for our update.  ' where Name = N'KYCReceived' and LangId = 1;
Update LangDetail set value =  N'私たちはあなたの個人的なKYC文書を受け取りました。私たちの更新を待ってください.  ' where Name = N'KYCReceived' and LangID = 2;
