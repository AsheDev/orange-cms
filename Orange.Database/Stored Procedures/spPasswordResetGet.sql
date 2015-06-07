USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordResetGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordResetGet
GO
CREATE PROCEDURE o.PasswordResetGet
	@UserId INT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	---
	SELECT TOP 1 FK_UserId, AuthenticationUrl, Created, Expires
	FROM o.PasswordResetDetails
	WHERE FK_UserId = @UserId
	ORDER BY Expires DESC;
GO