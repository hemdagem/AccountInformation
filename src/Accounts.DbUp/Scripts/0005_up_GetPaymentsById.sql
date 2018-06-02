IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_GetPaymentsById')
BEGIN
    exec ('create procedure up_GetPaymentsById as select 1')
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetPaymentsById]    Script Date: 11/12/2016 00:48:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_GetPaymentsById] 

@UserId uniqueidentifier

AS	
BEGIN
	
SELECT	Payments.Amount, Payments.PaidYearly,Payments.Id,
		Payments.Date,Payments.Title,Payments.Recurring, Income.PayDay
FROM    Payments  WITH (NOLOCK)  INNER JOIN
        
		Income  WITH (NOLOCK) ON Income.Id = Payments.IncomeId
WHERE	Income.Id=@UserId AND (Payments.Recurring=1 OR 
(Payments.Date < DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()),Income.PayDay) AND
 Payments.Date > DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()) -1,Income.PayDay)))
ORDER BY DATEPART(dd,Payments.Date) DESC


END

GO