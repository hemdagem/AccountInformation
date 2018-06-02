IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_GetUsers')
BEGIN
    exec ('create procedure up_GetUsers as select 1')
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetSummary]    Script Date: 11/12/2016 00:48:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_GetUsers] 

AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT * FROM Income

END


GO