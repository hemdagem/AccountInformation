/****** Object:  Table [dbo].[Income]    Script Date: 08/10/2015 21:31:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Income](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Income_Id]  DEFAULT (newid()),
	[Name] [nvarchar](max) NULL,
	[Amount] [money] NULL,
	[PayDay] [int] NULL,
 CONSTRAINT [PK_Income] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[Payments]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Payments_Id]  DEFAULT (newid()),
	[IncomeId] [uniqueidentifier] NOT NULL,
	[PaymentTypeId] [uniqueidentifier] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Amount] [money] NOT NULL,
	[PaidYearly] [bit] NOT NULL,
	[Recurring] [bit] NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
/****** Object:  Table [dbo].[PaymentType]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentType](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [DF_PaymentType_Id]  DEFAULT (newid()),
	[Title] [nvarchar](50) NULL,
 CONSTRAINT [PK_PaymentType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Income] FOREIGN KEY([IncomeId])
REFERENCES [dbo].[Income] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Income]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_PaymentType] FOREIGN KEY([PaymentTypeId])
REFERENCES [dbo].[PaymentType] ([Id])
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_PaymentType]
GO
/****** Object:  StoredProcedure [dbo].[up_AddPayment]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_AddPayment]
	-- Add the parameters for the stored procedure here
	@IncomeId uniqueidentifier,
	@Amount money,
	@Date datetime,
	@PaidYearly bit,
	@PaymentTypeId uniqueidentifier,
	@Recurring bit
AS
BEGIN
	
	INSERT INTO Payments (IncomeId,PaymentTypeId,PaidYearly, Amount, Date, Recurring) 
	OUTPUT Inserted.Id
	VALUES(@IncomeId,@PaymentTypeId,@PaidYearly,@Amount, @Date,@Recurring)

END



GO
/****** Object:  StoredProcedure [dbo].[up_AddPaymentType]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_AddPaymentType]
	-- Add the parameters for the stored procedure here
	@Title nvarchar(50)
AS
BEGIN
	
	INSERT INTO PaymentType (Title) 
	OUTPUT Inserted.Id
	VALUES(@Title)

END



GO
/****** Object:  StoredProcedure [dbo].[up_AddUser]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_AddUser] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(MAX),
	@Amount money,
	@PayDay int

AS
BEGIN
	
	INSERT INTO Income(Name,Amount, PayDay) 
	OUTPUT Inserted.Id
	VALUES(@Name, @Amount, @PayDay)

END


GO
/****** Object:  StoredProcedure [dbo].[up_DeletePayment]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_DeletePayment]
	
	@PaymentId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DELETE Payments WHERE Id =@PaymentId

	SELECT @@ROWCOUNT
END



GO
/****** Object:  StoredProcedure [dbo].[up_GetPaymentsById]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_GetPaymentsById] 

@UserId uniqueidentifier

AS	
BEGIN
	
SELECT	PaymentType.Title, Payments.Amount, Payments.PaidYearly,Payments.Id,
		Payments.Date,Payments.PaymentTypeId,Payments.Recurring, Income.PayDay
FROM    Payments INNER JOIN
        PaymentType ON PaymentType.Id = Payments.PaymentTypeId INNER JOIN
		Income ON Income.Id = Payments.IncomeId
WHERE	Income.Id=@UserId AND (Payments.Recurring=1 OR 
(Payments.Date < DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()),Income.PayDay) AND Payments.Date > DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()) -1,Income.PayDay)))
ORDER BY DATEPART(dd,Payments.Date) DESC


END



GO
/****** Object:  StoredProcedure [dbo].[up_GetPaymentType]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_GetPaymentType]
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier
AS
BEGIN
    -- Insert statements for procedure here
	SELECT Id, Title FROM PaymentType
	WHERE Id=@Id
END



GO
/****** Object:  StoredProcedure [dbo].[up_GetPaymentTypes]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_GetPaymentTypes]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, Title FROM PaymentType ORDER BY Title ASC
END




GO
/****** Object:  StoredProcedure [dbo].[up_GetUser]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_GetUser] 

@Id uniqueidentifier

AS	
BEGIN
	

	SELECT * FROM Income WHERE Id=@Id

END



GO
/****** Object:  StoredProcedure [dbo].[up_GetUsers]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_GetUsers] 

AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM Income

END



GO
/****** Object:  StoredProcedure [dbo].[up_UpdatePayment]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_UpdatePayment]
	-- Add the parameters for the stored procedure here
	
	@Amount money,
	@Date datetime,
	@PaidYearly bit,
	@PaymentTypeId uniqueidentifier,
	@Id uniqueidentifier,
	@Recurring bit

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Payments SET Amount=@Amount, Date=@Date, PaidYearly=@PaidYearly, PaymentTypeId=@PaymentTypeId, Recurring=@Recurring
	WHERE Id=@Id

	SELECT @@ROWCOUNT
END



GO
/****** Object:  StoredProcedure [dbo].[up_UpdatePaymentType]    Script Date: 08/10/2015 21:31:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[up_UpdatePaymentType]
	-- Add the parameters for the stored procedure here
	@Id uniqueidentifier,
	@Title nvarchar(50)
AS
BEGIN
	
	UPDATE PaymentType
	SET Title =@Title
	WHERE Id =@Id

END
