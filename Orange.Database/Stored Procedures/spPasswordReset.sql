USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordReset') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordReset
GO
CREATE PROCEDURE o.PasswordReset
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@AuthenticationURL NVARCHAR(128),
	@Created DATETIME,
	@Expires DATETIME
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		IF(GETUTCDATE() < (SELECT TOP 1 Expires FROM o.PasswordResetDetails WHERE FK_UserId = @UserId ORDER BY Expires DESC))
		BEGIN
			EXEC o.PasswordResetGet @UserId = @UserId;
		END
		ELSE
		BEGIN
			INSERT INTO o.PasswordResetDetails
			(FK_UserId, AuthenticationUrl, Created, Expires)
			VALUES
			(@UserId, @AuthenticationURL, @Created, @Expires);
			---			
			EXEC o.PasswordResetGet @UserId = @UserId;
		END
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
		EXEC o.PasswordResetGet @UserId = -99;
	END CATCH;
GO