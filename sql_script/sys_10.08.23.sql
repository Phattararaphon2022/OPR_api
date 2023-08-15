USE [OPR]
GO
/****** Object:  Table [dbo].[SYS_MT_COMBANkK]    Script Date: 10/08/2023 2:07:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SYS_MT_COMBANkK]') AND type in (N'U'))
DROP TABLE [dbo].[SYS_MT_COMBANkK]
GO
/****** Object:  Table [dbo].[SYS_MT_COMADDRES]    Script Date: 10/08/2023 2:07:33 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SYS_MT_COMADDRES]') AND type in (N'U'))
DROP TABLE [dbo].[SYS_MT_COMADDRES]
GO
/****** Object:  Table [dbo].[SYS_MT_COMADDRES]    Script Date: 10/08/2023 2:07:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYS_MT_COMADDRES](
	[COMPANY_CODE] [varchar](5) NOT NULL,
	[COMBRANCH_CODE] [varchar](5) NOT NULL,
	[COMADDRES_TYPE] [char](3) NOT NULL,
	[COMADDRES_NOTH] [varchar](30) NULL,
	[COMADDRES_MOOTH] [varchar](30) NULL,
	[COMADDRES_SOITH] [varchar](100) NULL,
	[COMADDRES_ROADTH] [varchar](100) NULL,
	[COMADDRES_TAMBONTH] [varchar](100) NULL,
	[COMADDRES_AMPHURTH] [varchar](100) NULL,
	[COMADDRES_NOEN] [varchar](30) NULL,
	[COMADDRES_MOOEN] [varchar](30) NULL,
	[COMADDRES_SOIEN] [varchar](100) NULL,
	[COMADDRES_ROADEN] [varchar](100) NULL,
	[COMADDRES_TAMBONEN] [varchar](100) NULL,
	[COMADDRES_AMPHUREN] [varchar](100) NULL,
	[PROVINCE_CODE] [varchar](30) NULL,
	[COMADDRES_ZIPCODE] [varchar](30) NULL,
	[COMADDRES_TEL] [varchar](30) NULL,
	[COMADDRES_FAX] [varchar](100) NULL,
	[COMADDRES_URL] [varchar](100) NULL,
	[COMADDRES_EMAIL] [varchar](50) NULL,
	[COMADDRES_LINE] [varchar](50) NULL,
	[COMADDRES_FACEBOOK] [varchar](50) NULL,
	[CREATED_BY] [varchar](20) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](20) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[FLAG] [bit] NOT NULL,
 CONSTRAINT [PK_SYS_MT_COMADDRES] PRIMARY KEY CLUSTERED 
(
	[COMPANY_CODE] ASC,
	[COMBRANCH_CODE] ASC,
	[COMADDRES_TYPE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SYS_MT_COMBANkK]    Script Date: 10/08/2023 2:07:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SYS_MT_COMBANkK](
	[COMPANY_CODE] [varchar](5) NOT NULL,
	[COMBANK_ID] [int] NOT NULL,
	[COMBANK_BANKCODE] [varchar](20) NOT NULL,
	[COMBANK_BANKNAME] [varchar](20) NOT NULL,
	[COMBANK_BANKACCOUNT] [varchar](20) NOT NULL,
	[COMBANK_BANKTYPE] [varchar](20) NOT NULL,
	[COMBANK_BRANCH] [varchar](20) NOT NULL,
	[CREATED_BY] [varchar](20) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](20) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[FLAG] [bit] NOT NULL,
 CONSTRAINT [PK_SYS_MT_COMBANKk] PRIMARY KEY CLUSTERED 
(
	[COMPANY_CODE] ASC,
	[COMBANK_BANKACCOUNT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[SYS_MT_COMADDRES] ([COMPANY_CODE], [COMBRANCH_CODE], [COMADDRES_TYPE], [COMADDRES_NOTH], [COMADDRES_MOOTH], [COMADDRES_SOITH], [COMADDRES_ROADTH], [COMADDRES_TAMBONTH], [COMADDRES_AMPHURTH], [COMADDRES_NOEN], [COMADDRES_MOOEN], [COMADDRES_SOIEN], [COMADDRES_ROADEN], [COMADDRES_TAMBONEN], [COMADDRES_AMPHUREN], [PROVINCE_CODE], [COMADDRES_ZIPCODE], [COMADDRES_TEL], [COMADDRES_FAX], [COMADDRES_URL], [COMADDRES_EMAIL], [COMADDRES_LINE], [COMADDRES_FACEBOOK], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE], [FLAG]) VALUES (N'OPR', N'00000', N'   ', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'17', N'3', N'3', N'3', N'3', N'3', N'3', N'3', N'Admin', CAST(N'2023-08-09T11:57:01.517' AS DateTime), N'Admin', CAST(N'2023-08-09T15:59:44.963' AS DateTime), 0)
INSERT [dbo].[SYS_MT_COMADDRES] ([COMPANY_CODE], [COMBRANCH_CODE], [COMADDRES_TYPE], [COMADDRES_NOTH], [COMADDRES_MOOTH], [COMADDRES_SOITH], [COMADDRES_ROADTH], [COMADDRES_TAMBONTH], [COMADDRES_AMPHURTH], [COMADDRES_NOEN], [COMADDRES_MOOEN], [COMADDRES_SOIEN], [COMADDRES_ROADEN], [COMADDRES_TAMBONEN], [COMADDRES_AMPHUREN], [PROVINCE_CODE], [COMADDRES_ZIPCODE], [COMADDRES_TEL], [COMADDRES_FAX], [COMADDRES_URL], [COMADDRES_EMAIL], [COMADDRES_LINE], [COMADDRES_FACEBOOK], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE], [FLAG]) VALUES (N'OPR', N'1212', N'   ', N'1', N'1', N'1', N'1', N'1', N'1', N'1', N'1', N'1', N'1', N'1', N'1', N'17', N'1', N'1', N'1', N'11', N'1', N'1', N'1', N'Admin', CAST(N'2023-08-09T15:36:13.413' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[SYS_MT_COMBANkK] ([COMPANY_CODE], [COMBANK_ID], [COMBANK_BANKCODE], [COMBANK_BANKNAME], [COMBANK_BANKACCOUNT], [COMBANK_BANKTYPE], [COMBANK_BRANCH], [CREATED_BY], [CREATED_DATE], [MODIFIED_BY], [MODIFIED_DATE], [FLAG]) VALUES (N'OPR', 1, N'BKK', N'ชื่อบัญชี', N'สาขา', N'ออมทรัพย์', N'12345', N'Admin', CAST(N'2023-08-09T15:59:45.297' AS DateTime), NULL, NULL, 0)
GO
