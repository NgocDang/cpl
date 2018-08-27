insert into LangDetail values (1, N'RegisterActivate',  N'You are now registered. Please click on the button below to activate your account');
insert into LangDetail values (2, N'RegisterActivate',  N'この度はCPLプロジェクトへのご参加誠に有難うございます。    お客様のご登録はまだ完了致しておりません。  以下の「アクティブ化する」をクリックして頂きアカウントの本登録をお願い致します。  ');

insert into LangDetail values (1, N'PreviouslyActivated',  N'Your account is activated previously.');
insert into LangDetail values (2, N'PreviouslyActivated',  N'お客様のアカウントは以前に有効化されています。');

insert into LangDetail values (1, N'RequestNotExist',  N'Request does not exist. Please try again.');
insert into LangDetail values (2, N'RequestNotExist',  N'リクエストは存在しません。再度お試し下さい。');

insert into LangDetail values (1, N'ClickHereToAskResetPassword',  N'Click <a href=''/Authentication/ForgotPassword''>here</a> to submit a new request to reset password');
insert into LangDetail values (2, N'ClickHereToAskResetPassword',  N'パスワードのリセットは <a href=''/Authentication/ForgotPassword''>こちら</a>をクリック');

insert into LangDetail values (1, N'ExpiredResetPasswordToken',  N'Reset password token is expired.');
insert into LangDetail values (2, N'ExpiredResetPasswordToken',  N'パスワードの確認メールは期限切れです。');

insert into LangDetail values (1, N'ClickHereToLogIn',  N'Click <a href=''/Authentication/LogIn''>here</a> to log in');
insert into LangDetail values (2, N'ClickHereToLogIn',  N'ログインは<a href=''/Authentication/LogIn''>こちら</a>をクリック');

insert into LangDetail values (1, N'ExpiredActivateToken',  N'Activate link is expired.');
insert into LangDetail values (2, N'ExpiredActivateToken',  N'アクティベートトークンは期限切れです。');

insert into LangDetail values (1, N'AccountIsActivated',  N'Account is activated successfully.');
insert into LangDetail values (2, N'AccountIsActivated',  N'アカウントは、有効になりました。');

insert into LangDetail values (1, N'ClickHereToReturnToTopPage',  N'Click <a href=''/Home/Index''>here</a> to return to top page');
insert into LangDetail values (2, N'ClickHereToReturnToTopPage',  N'トップページに<a href=''/Home/Index''>戻る</a>');

insert into LangDetail values (1, N'ClickHereToRequestNewActivateToken',  N'Click <a href=''/Authentication/Resend''>here</a> to request a new activate link');
insert into LangDetail values (2, N'ClickHereToRequestNewActivateToken',  N'確認メールの再送信は<a href=''/Authentication/Resend''>こちら</a>');

insert into LangDetail values (1, N'InvalidToken',  N'Invalid token. Please try again.');
insert into LangDetail values (2, N'InvalidToken',  N'無効なトークンです。再度お試しください。');

insert into LangDetail values (1, N'ResendActivateCode',  N'Resend activate link');
insert into LangDetail values (2, N'ResendActivateCode',  N'確認メールを再送信する。');

insert into LangDetail values (1, N'ResendActivateCodeDesc',  N'We will send a new link to activate your account');
insert into LangDetail values (2, N'ResendActivateCodeDesc',  N'アカウントの確認メールを送ります。');

insert into LangDetail values (1, N'InvalidOrNonExistingEmail',  N'Invalid or non-existing email');
insert into LangDetail values (2, N'InvalidOrNonExistingEmail',  N'無効、もしくは存在しないメールアドレスです。');

insert into LangDetail values (1, N'Resend',  N'Resend');
insert into LangDetail values (2, N'Resend',  N'再送信');

insert into LangDetail values (1, N'NewActivateCodeSent',  N'New activate link has been sent to your email. Please check and activate your account.');
insert into LangDetail values (2, N'NewActivateCodeSent',  N'新しいアカウント確認メールが送信されました。メールを確認して、アカウントを有効にしてください。');

update LangDetail set value = N'An activate link has been sent to your email. Please check and activate your account.' where Name = N'ActivateEmailSent' and LangId = 1;
update LangDetail set value = N'確認メールが送信されました。 リンクをクリックしてアカウントを有効にしてください。' where Name = N'ActivateEmailSent' and LangId = 2;
