INSERT INTO MobileLangDetail VALUES (1, N'QrScanScreen_Title', N'Qr Code Scanner');
INSERT INTO MobileLangDetail VALUES (2, N'QrScanScreen_Title', N'Qrコードスキャナ');

INSERT INTO MobileLangDetail VALUES (1, N'QrScanScreen_Dialog_Permisson_Title', N'Permisson denied');
INSERT INTO MobileLangDetail VALUES (2, N'QrScanScreen_Dialog_Permisson_Title', N'アクセス拒否');

INSERT INTO MobileLangDetail VALUES (1, N'QrScanScreen_Dialog_Permisson_Content', N'"Crypto Odds" Would like to Access the Camera');
INSERT INTO MobileLangDetail VALUES (2, N'QrScanScreen_Dialog_Permisson_Content', N'「暗号違法」はカメラにアクセスしたい');

INSERT INTO MobileLangDetail VALUES (1, N'QrScanScreen_Dialog_Permisson_Content2', N'Please accept camera permission to scan QR code');
INSERT INTO MobileLangDetail VALUES (2, N'QrScanScreen_Dialog_Permisson_Content2', N'QRコードをスキャンするためのカメラの許可を受け入れてください');

INSERT INTO MobileLangDetail VALUES (1, N'QrScanScreen_Dialog_Button_Cancel', N'Cancel');
INSERT INTO MobileLangDetail VALUES (2, N'QrScanScreen_Dialog_Button_Cancel', N'キャンセル');

INSERT INTO MobileLangDetail VALUES (1, N'QrScanScreen_Dialog_Button_Ok', N'OK');
INSERT INTO MobileLangDetail VALUES (2, N'QrScanScreen_Dialog_Button_Ok', N'はい');

INSERT INTO MobileLangDetail VALUES (1, N'EditEmailScreen_Input_Confirm_Email_Placeholder', N'Confirm new email');
INSERT INTO MobileLangDetail VALUES (2, N'EditEmailScreen_Input_Confirm_Email_Placeholder', N'新しいメールを確認する');

INSERT INTO MobileLangDetail VALUES (1, N'EditEmailScreen_Existing_Email', N'Email existing in system');
INSERT INTO MobileLangDetail VALUES (2, N'EditEmailScreen_Existing_Email', N'システムに存在する電子メール');

INSERT INTO MobileLangDetail VALUES (1, N'EditEmailScreen_Email_Updated_Successfully', N'Email updated successfully!');
INSERT INTO MobileLangDetail VALUES (2, N'EditEmailScreen_Email_Updated_Successfully', N'電子メールを正常に更新');

INSERT INTO MobileLangDetail VALUES (1, N'KYCScreen_Dialog_Title', N'If you do not KYC for your account. Please touch "Go to Web" for update KYC');
INSERT INTO MobileLangDetail VALUES (2, N'KYCScreen_Dialog_Title', N'あなたのアカウントにKYCがない場合。 KYCを更新するには、[ウェブにアクセス]をタップしてください');

INSERT INTO MobileLangDetail VALUES (1, N'KYCScreen_Button_Cancel', N'Cancel');
INSERT INTO MobileLangDetail VALUES (2, N'KYCScreen_Button_Cancel', N'キャンセル');

INSERT INTO MobileLangDetail VALUES (1, N'KYCScreen_Button_Go_To_Web', N'Go to web');
INSERT INTO MobileLangDetail VALUES (2, N'KYCScreen_Button_Go_To_Web', N'ウェブに行く');

UPDATE [dbo].[MobileLangDetail] SET [Value] = N'Security' WHERE [Name] = 'UserAccountScreen_Button_Security', [LangId] = 1