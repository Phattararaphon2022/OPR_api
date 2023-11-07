USE [OPR]
GO
/****** Object:  Table [dbo].[PRO_TR_PROIMAGES]    Script Date: 07/11/2023 5:27:52 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PRO_TR_PROIMAGES]') AND type in (N'U'))
DROP TABLE [dbo].[PRO_TR_PROIMAGES]
GO
/****** Object:  Table [dbo].[PRO_TR_DOCATT]    Script Date: 07/11/2023 5:27:52 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PRO_TR_DOCATT]') AND type in (N'U'))
DROP TABLE [dbo].[PRO_TR_DOCATT]
GO
/****** Object:  Table [dbo].[PRO_TR_DOCATT]    Script Date: 07/11/2023 5:27:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRO_TR_DOCATT](
	[COMPANY_CODE] [varchar](30) NOT NULL,
	[PROJECT_CODE] [varchar](15) NOT NULL,
	[DOCUMENT_ID] [bigint] NOT NULL,
	[JOB_TYPE] [varchar](100) NOT NULL,
	[DOCUMENT_NAME] [varchar](100) NOT NULL,
	[DOCUMENT_TYPE] [varchar](100) NOT NULL,
	[DOCUMENT_PATH] [varchar](100) NOT NULL,
	[CREATED_BY] [varchar](20) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_PRO_TR_DOCATT] PRIMARY KEY CLUSTERED 
(
	[COMPANY_CODE] ASC,
	[PROJECT_CODE] ASC,
	[DOCUMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PRO_TR_PROIMAGES]    Script Date: 07/11/2023 5:27:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PRO_TR_PROIMAGES](
	[COMPANY_CODE] [varchar](20) NOT NULL,
	[PROJECT_CODE] [varchar](15) NOT NULL,
	[PROIMAGES_NO] [int] NOT NULL,
	[PROIMAGES_IMAGES] [image] NOT NULL,
	[CREATED_BY] [varchar](20) NOT NULL,
	[CREATED_DATE] [datetime] NOT NULL,
	[MODIFIED_BY] [varchar](20) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[FLAG] [bit] NOT NULL,
 CONSTRAINT [PK_PRO_TR_PROIMAGES] PRIMARY KEY CLUSTERED 
(
	[COMPANY_CODE] ASC,
	[PROJECT_CODE] ASC,
	[PROIMAGES_NO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
