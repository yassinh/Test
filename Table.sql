USE [Test]
GO

/****** Object:  Table [dbo].[DevTests]    Script Date: 1/5/2017 3:18:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DevTests](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CampaignName] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Clicks] [int] NULL,
	[Impressions] [int] NULL,
	[AffiliateName] [nvarchar](max) NULL,
	[Conversions] [int] NULL,
 CONSTRAINT [PK_dbo.DevTests] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

