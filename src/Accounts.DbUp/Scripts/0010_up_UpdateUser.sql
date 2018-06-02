IF NOT EXISTS(SELECT 1 FROM sys.procedures  WHERE Name = 'up_UpdateUser')
BEGIN
    exec ('create procedure up_UpdateUser as select 1')
END
GO
/****** Object:  StoredProcedure [dbo].[up_AddUser]    Script Date: 11/12/2016 00:48:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[up_UpdateUser] 
	-- Add the parameters for the stored procedure here
	@Name nvarchar(MAX),
	@Amount money,
	@PayDay int,
	@Id uniqueidentifier

AS
BEGIN
	
	UPDATE Income
	SET Name =@Name, Amount =@Amount, PayDay = @PayDay
	WHERE Id=@Id

	SELECT @@ROWCOUNT
END

GO