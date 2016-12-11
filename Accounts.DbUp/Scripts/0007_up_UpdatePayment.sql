IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_UpdatePayment')
BEGIN
    exec ('create procedure up_UpdatePayment as select 1')
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetSummary]    Script Date: 11/12/2016 00:48:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[up_UpdatePayment]
	-- Add the parameters for the stored procedure here
	
	@Amount money,
	@Date datetime,
	@PaidYearly bit,
	@Id uniqueidentifier,
	@Recurring bit,
	@Title nvarchar(max)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE Payments SET Amount=@Amount, Date=@Date, PaidYearly=@PaidYearly, Recurring=@Recurring, Title=@Title
	WHERE Id=@Id

	SELECT @@ROWCOUNT
END


GO