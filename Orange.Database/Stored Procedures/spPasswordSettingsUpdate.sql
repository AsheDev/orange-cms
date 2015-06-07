USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordSettingsUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordSettingsUpdate
GO
CREATE PROCEDURE o.PasswordSettingsUpdate
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@MaxAttempts INT,
	@ExpirationInDays INT,
	@ResetExpirationInMinutes INT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.PasswordSettings
		SET MaxPasswordAttempts = @MaxAttempts,
			ExpirationInDays = @ExpirationInDays,
			ResetExpirationInMinutes = @ResetExpirationInMinutes
		---
		EXEC o.PasswordSettingsGet;
		---
		COMMIT;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() = -1 ROLLBACK;
		DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
		SET @ErrorMessage = CAST(ERROR_NUMBER() AS VARCHAR) + ': ' + ERROR_MESSAGE();
		SET @ErrorSeverity = ERROR_SEVERITY();
		SET @ErrorState = ERROR_STATE();
		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
		SELECT -1, -1, -1 -- FAILURE (there is really only one record to be dealing with)
	END CATCH;
GO