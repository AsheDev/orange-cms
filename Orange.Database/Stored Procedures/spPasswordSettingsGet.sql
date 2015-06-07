USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordSettingsGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordSettingsGet
GO
CREATE PROCEDURE o.PasswordSettingsGet
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	SELECT MaxPasswordAttempts, ExpirationInDays, ResetExpirationInMinutes
	FROM o.PasswordSettings;
GO