USE [CPL]
GO
/****** Object:  Table [dbo].[CoinTransaction]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CoinTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SysUserId] [int] NOT NULL,
	[FromWalletAddress] [nvarchar](100) NULL,
	[ToWalletAddress] [nvarchar](100) NOT NULL,
	[CoinAmount] [money] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[Status] [nvarchar](20) NOT NULL,
	[TokenAmount] [money] NULL,
	[Rate] [float] NULL,
 CONSTRAINT [PK_Transaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Currency]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currency](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Currency] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Game]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_GameType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GameHistory]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GameHistory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GameId] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedDate] [datetime] NULL,
	[Amount] [money] NOT NULL,
	[Result] [bit] NULL,
	[Bonus] [money] NULL,
	[SysUserId] [int] NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lang]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lang](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](10) NULL,
	[Image] [nvarchar](50) NULL,
 CONSTRAINT [PK_Lang] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LangDetail]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LangDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LangId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [ntext] NULL,
 CONSTRAINT [PK_LangDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LangMsgDetail]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LangMsgDetail](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LangId] [int] NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [ntext] NULL,
 CONSTRAINT [PK_LangMsgDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](400) NULL,
	[CreatedDate] [datetime] NULL,
	[LangId] [int] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rate]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rate](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Value] [float] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[CurrencyId] [int] NOT NULL,
 CONSTRAINT [PK_Rate] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Value] [nvarchar](100) NULL,
	[Description] [nvarchar](200) NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SysUser]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SysUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[FirstName] [nvarchar](30) NULL,
	[LastName] [nvarchar](30) NULL,
	[Address] [nvarchar](200) NULL,
	[ETHWalletAddress] [nvarchar](100) NULL,
	[Mobile] [nvarchar](20) NULL,
	[ResetPasswordToken] [nvarchar](50) NULL,
	[ResetPasswordDate] [datetime] NULL,
	[ActivateToken] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[KYCVerified] [bit] NULL,
	[FrontSide] [nvarchar](50) NULL,
	[BackSide] [nvarchar](50) NULL,
	[DOB] [datetime] NULL,
	[BTCHDWalletAddressIndex] [int] NOT NULL,
	[BTCWalletAddress] [nvarchar](100) NULL,
	[KYCCreatedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[ETHHDWalletAddress] [nvarchar](100) NULL,
	[ETHHDWalletAddressIndex] [int] NOT NULL,
	[BTCHDWalletAddress] [nvarchar](100) NULL,
	[BTCWallet] [money] NOT NULL,
	[ETHWallet] [money] NOT NULL,
	[TokenWallet] [money] NOT NULL,
 CONSTRAINT [PK_SysUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 6/26/2018 5:03:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Designation] [nvarchar](50) NULL,
	[Avatar] [nvarchar](150) NULL,
 CONSTRAINT [PK_Team] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Template]    Script Date: 6/26/2018 5:03:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Template](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Body] [nvarchar](50) NULL,
	[Subject] [nvarchar](200) NULL,
 CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Currency] ON 

INSERT [dbo].[Currency] ([Id], [Name]) VALUES (1, N'ETH')
INSERT [dbo].[Currency] ([Id], [Name]) VALUES (2, N'BTC')
SET IDENTITY_INSERT [dbo].[Currency] OFF
SET IDENTITY_INSERT [dbo].[Game] ON 

INSERT [dbo].[Game] ([Id], [Name], [Description], [IsActive]) VALUES (1, N'Lotto', NULL, 1)
INSERT [dbo].[Game] ([Id], [Name], [Description], [IsActive]) VALUES (2, N'BTCPrise', NULL, 1)
INSERT [dbo].[Game] ([Id], [Name], [Description], [IsActive]) VALUES (3, N'WorldCup', NULL, 1)
SET IDENTITY_INSERT [dbo].[Game] OFF
SET IDENTITY_INSERT [dbo].[Lang] ON 

INSERT [dbo].[Lang] ([Id], [Name], [Image]) VALUES (1, N'English', N'flag-icon-us')
INSERT [dbo].[Lang] ([Id], [Name], [Image]) VALUES (2, N'日本語', N'flag-icon-jp')
INSERT [dbo].[Lang] ([Id], [Name], [Image]) VALUES (3, N'한국어
', N'flag-icon-kr')
INSERT [dbo].[Lang] ([Id], [Name], [Image]) VALUES (4, N'中文（简体）
', N'flag-icon-cn')
INSERT [dbo].[Lang] ([Id], [Name], [Image]) VALUES (5, N'中文（传统）', N'flag-icon-cn')
SET IDENTITY_INSERT [dbo].[Lang] OFF
SET IDENTITY_INSERT [dbo].[LangDetail] ON 

INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (1, 1, N'TokenSymbol', N'CPL')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (2, 1, N'TokenTeam', N'CPL Team')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (3, 1, N'ProfileEdit', N'Edit Profile')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (4, 1, N'CredentialsEdit', N'Edit Credentials')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (5, 1, N'LogOut', N'Log out')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (6, 1, N'LogIn', N'Log in')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (7, 1, N'Email', N'Email')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (8, 1, N'EmailRequired', N'Please fill in your email')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (9, 1, N'Password', N'Password')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (10, 1, N'ForgotPassword', N'Forgot password?')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (11, 1, N'PasswordRequired', N'Please fill in your password')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (12, 1, N'InvalidEmailPassword', N'Invalid email or password. Please try again.')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (13, 1, N'NoAccount', N'Don''t have an account yet?')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (14, 1, N'Copyright', N'Copyright')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (15, 1, N'Register', N'Register')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (16, 1, N'CreateAccount', N'Create an account')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (17, 1, N'ChangeLanguage', N'Change Language')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (18, 1, N'EmailInactivatingAccount', N'In-activating account. Please check your email to activate!')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (19, 1, N'AccountInfoRequired', N'Please fill in your account information')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (20, 1, N'PasswordConfirm', N'Password confirm')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (21, 1, N'MismatchPassword', N'Mismatch password')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (22, 1, N'AlreadyHaveAccount', N'Already have an account?')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (23, 1, N'ClickHere', N'Click here')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (24, 1, N'ToLogIn', N'to log in')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (25, 1, N'ResendActivateCodeLinkDesc', N'Click <a href="/Authentication/Resend">here</a> to resend activate email. ')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (26, 1, N'Dashboard', N'Dashboard')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (27, 1, N'ErrorOccurs', N'Error occurs')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (28, 1, N'ExistingEmail', N'Existing email, please use another.')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (29, 1, N'IsAccountActivationEnable', N'Is account activation enable?')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (30, 1, N'RegistrationSuccessful', N'Registration successful')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (31, 1, N'ActivateEmailSent', N'An email has been sent to your registered email address')
INSERT [dbo].[LangDetail] ([Id], [LangId], [Name], [Value]) VALUES (32, 1, N'ActivationSuccessful', N'Activation successful')
SET IDENTITY_INSERT [dbo].[LangDetail] OFF
SET IDENTITY_INSERT [dbo].[Rate] ON 

INSERT [dbo].[Rate] ([Id], [Value], [StartDate], [EndDate], [CurrencyId]) VALUES (1, 1000, CAST(N'2018-06-26T00:00:00.000' AS DateTime), CAST(N'2018-07-26T00:00:00.000' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Rate] OFF
SET IDENTITY_INSERT [dbo].[Setting] ON 

INSERT [dbo].[Setting] ([Id], [Name], [Value], [Description]) VALUES (1, N'IsOnMaintenance', N'false', NULL)
INSERT [dbo].[Setting] ([Id], [Name], [Value], [Description]) VALUES (2, N'IsAccountActivationEnable', N'true', NULL)
SET IDENTITY_INSERT [dbo].[Setting] OFF
SET IDENTITY_INSERT [dbo].[SysUser] ON 

INSERT [dbo].[SysUser] ([Id], [IsAdmin], [Email], [Password], [FirstName], [LastName], [Address], [ETHWalletAddress], [Mobile], [ResetPasswordToken], [ResetPasswordDate], [ActivateToken], [CreatedDate], [KYCVerified], [FrontSide], [BackSide], [DOB], [BTCHDWalletAddressIndex], [BTCWalletAddress], [KYCCreatedDate], [IsDeleted], [ETHHDWalletAddress], [ETHHDWalletAddressIndex], [BTCHDWalletAddress], [BTCWallet], [ETHWallet], [TokenWallet]) VALUES (20, 0, N'nlqnhat@gmail.com', N'$2a$10$cx5UpTFFTj0LCDnMlhxrge3HS8UkJ3qKE5MbwtW/vKuz2tPsJ2HOC', NULL, NULL, NULL, NULL, NULL, NULL, NULL, N'26b81c95-4a21-49a7-97dc-9f9c8cc95928', CAST(N'2018-06-26T16:38:16.700' AS DateTime), NULL, NULL, NULL, NULL, 0, NULL, NULL, 0, NULL, 0, NULL, 0.0000, 0.0000, 0.0000)
SET IDENTITY_INSERT [dbo].[SysUser] OFF
SET IDENTITY_INSERT [dbo].[Template] ON 

INSERT [dbo].[Template] ([Id], [Name], [Body], [Subject]) VALUES (1, N'Activate', N'ActivateEmailTemplate', N'Activate your account at CPL')
INSERT [dbo].[Template] ([Id], [Name], [Body], [Subject]) VALUES (2, N'Member', N'MemberEmailTemplate', N'Account is activated successfully at CPL')
SET IDENTITY_INSERT [dbo].[Template] OFF
ALTER TABLE [dbo].[SysUser] ADD  CONSTRAINT [DF_SysUser_BTCHDWalletAddressIndex]  DEFAULT ((0)) FOR [BTCHDWalletAddressIndex]
GO
ALTER TABLE [dbo].[SysUser] ADD  CONSTRAINT [DF_SysUser_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SysUser] ADD  CONSTRAINT [DF_SysUser_ETHHDWalletAddressIndex]  DEFAULT ((0)) FOR [ETHHDWalletAddressIndex]
GO
