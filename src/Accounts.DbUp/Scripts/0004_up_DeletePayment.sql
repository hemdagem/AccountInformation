IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_DeletePayment')
BEGIN
    exec ('create procedure up_DeletePayment as select 1')
END
GO

/****** Object:  StoredProcedure [dbo].[up_DeletePayment]    Script Date: 11/12/2016 00:48:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[up_DeletePayment]
	
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