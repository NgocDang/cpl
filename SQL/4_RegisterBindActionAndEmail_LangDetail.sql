Use CPL;

-- Lang detail
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'ErrorOccurs', N'Error occurs');
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'ExistingEmail', N'Existing email, please use another.');
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'RegistrationSuccessful', N'Registration successful');
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'ActivateEmailSent', N'An email has been sent to your registered email address');
INSERT [dbo].[LangDetail] ([LangId], [Name], [Value]) VALUES (1, N'ActivationSuccessful', N'Activation successful');

-- Template
INSERT [dbo].[Template] ([Name], [Body], [Subject]) VALUES (N'Activate', N'ActivateEmailTemplate', N'Activate your account at CPL');
INSERT [dbo].[Template] ([Name], [Body], [Subject]) VALUES (N'Member', N'MemberEmailTemplate', N'Account is activated successfully at CPL');

-- Setting
INSERT [dbo].[Setting] ([Name], [Value]) VALUES (N'IsAccountActivationEnable', N'false');