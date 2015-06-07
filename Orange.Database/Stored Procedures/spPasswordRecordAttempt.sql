USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordRecordAttempt') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordRecordAttempt
GO
CREATE PROCEDURE o.PasswordRecordAttempt
	@Username NVARCHAR(256)
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	---
	BEGIN TRY
		---
		DECLARE @MaxAttempts INT = (SELECT MaxPasswordAttempts FROM o.PasswordSettings);
		DECLARE @NewAttempts INT = (SELECT Attempts FROM o.Passwords AS P INNER JOIN o.Users AS U ON U.Id = P.FK_UserId WHERE U.Email = @Username OR U.Name = @Username) + 1;
		---
		UPDATE P
		SET Attempts = @NewAttempts
		FROM o.Passwords AS P
		INNER JOIN o.Users AS U
		ON U.Id = P.FK_UserId
		WHERE U.Email = @Username
		OR U.Name = @Username;
		---
		SELECT @NewAttempts;
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
		SELECT -1;
	END CATCH;
GO