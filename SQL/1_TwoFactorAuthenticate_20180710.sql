Use CPL;

ALTER TABLE SysUser
  ADD TwoFactorAuthenticationEnable bit not null DEFAULT (0);

Insert into LangDetail Values (1, N'', N'');
Insert into LangDetail Values (1, N'TwoFactorAuthentication', N'Two-factor authentication');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationShortDescription', N'Please follow the below steps for two-factor authentication:');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationStep1', N'Install the authentication application on smartphone:');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationStep2', N'iOS (App Store)');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationStep3', N'Android (Google Play)');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationStep4', N'Scan the QR code with the authentication application.');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationStep5', N'Enter the 6 digit number displayed in the application and click on activate two-factor verification.');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationStep6', N'Two-step authentication code will be required from the next login.');
Insert into LangDetail Values (1, N'Disable', N'Disable');
Insert into LangDetail Values (1, N'InvalidPIN', N'Invalid PIN. Please try again!');
Insert into LangDetail Values (1, N'EnterPINToEnable', N'Enter PIN to enable');
Insert into LangDetail Values (1, N'Enable', N'Enable');
Insert into LangDetail Values (1, N'TwoFactorAuthenticationUpdated', N'Two-factor authentication setting is updated.');
Insert into LangDetail Values (1, N'PIN', N'PIN');
Insert into LangDetail Values (1, N'WaitingPIN', N'Waiting for PIN to be input');

