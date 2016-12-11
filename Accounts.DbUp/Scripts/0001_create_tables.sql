SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS(SELECT 1 FROM sys.tables  WHERE Name = 'Income')
BEGIN
/****** Object:  Table [dbo].[Income]    Script Date: 11/12/2016 00:34:16 ******/
CREATE TABLE [dbo].[Income](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Amount] [decimal](19, 4) NULL,
	[PayDay] [int] NULL,
 CONSTRAINT [PK_Income] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT 1 FROM sys.tables  WHERE Name = 'Payments')
BEGIN
CREATE TABLE [dbo].[Payments](
	[Id] [uniqueidentifier] NOT NULL,
	[IncomeId] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Amount] [decimal](19, 4) NOT NULL,
	[PaidYearly] [bit] NOT NULL,
	[Recurring] [bit] NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Income] FOREIGN KEY([IncomeId])
REFERENCES [dbo].[Income] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Income]
GO
