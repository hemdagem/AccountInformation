IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_AddPayment')
BEGIN
    exec ('create procedure up_AddPayment as select 1')
END
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[up_AddPayment]
	-- Add the parameters for the stored procedure here
	@IncomeId uniqueidentifier,
	@Amount money,
	@Date datetime,
	@PaidYearly bit,
	@Title nvarchar(max),
	@Recurring bit
AS
BEGIN
	
	INSERT INTO Payments (IncomeId,Title,PaidYearly, Amount, Date, Recurring) 
	OUTPUT Inserted.Id
	VALUES(@IncomeId,@Title,@PaidYearly,@Amount, @Date,@Recurring)

END


GO