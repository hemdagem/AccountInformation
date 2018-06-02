IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_GetUser')
BEGIN
    exec ('create procedure up_GetUser as select 1')
END
GO

/****** Object:  StoredProcedure [dbo].[up_GetSummary]    Script Date: 11/12/2016 00:48:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[up_GetUser] 

@Id uniqueidentifier

AS	
BEGIN
	
	SELECT * FROM Income WITH (NOLOCK) WHERE Id=@Id

END


GO