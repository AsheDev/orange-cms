USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordUpdate
GO
CREATE PROCEDURE o.PasswordUpdate
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@HashedPassword NVARCHAR(128) = NULL,
	@Salt NVARCHAR(128) = NULL,
	@Expires BIT,
	@Expiration DATETIME,
	@Attempts INT,
	@IsLocked BIT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		IF(@HashedPassword IS NULL)
		BEGIN
			UPDATE o.Passwords
			SET Attempts = @Attempts,
				Expires = @Expires,
				Expiration = @Expiration,
				IsLocked = @IsLocked
			WHERE FK_UserId = @UserId;
		END
		ELSE
		BEGIN
			UPDATE o.Passwords
			SET Attempts = @Attempts,
				Salt = @Salt,
				HashedPassword = @HashedPassword,
				Expires = @Expires,
				Expiration = @Expiration,
				IsLocked = @IsLocked
			WHERE FK_UserId = @UserId;
		END
		---
		EXEC o.PasswordGet @UserId = @UserId;
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
		EXEC o.PasswordGet @UserId = -99;
	END CATCH;
GO