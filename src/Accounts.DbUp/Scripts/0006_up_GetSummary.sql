IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_GetSummary')
BEGIN
    exec ('create procedure up_GetSummary as select 1')
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
ALTER PROCEDURE [dbo].[up_GetSummary] 

@UserId uniqueidentifier

AS	
BEGIN
	
SELECT		I.Amount, I.Amount - SUM(P.Amount) AS RemainingEachMonth, (SELECT SUM(PS.Amount) FROM Payments PS WHERE PS.PaidYearly=1 AND PS.IncomeId=I.Id) AS YearlyAmountEachMonth,
			(SELECT SUM(PS.Amount) FROM Payments PS WHERE PS.PaidYearly=0 AND PS.IncomeId=I.Id AND DATEPART(DD,PS.Date) >DATEPART(DD,GETDATE())) AS CurrentAmountToPayThisMonth
FROM		Income I  WITH (NOLOCK)  INNER JOIN 
			Payments P  WITH (NOLOCK)  ON P.IncomeId = I.Id
WHERE	IncomeId=@UserId
GROUP BY	I.Amount, I.Id

END


GO