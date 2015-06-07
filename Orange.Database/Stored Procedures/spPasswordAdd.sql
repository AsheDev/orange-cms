USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordAdd') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordAdd
GO
CREATE PROCEDURE o.PasswordAdd
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@HashedPassword NVARCHAR(128),
	@Salt NVARCHAR(128),
	@Expires BIT,
	@Expiration DATETIME
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		INSERT INTO o.Passwords
		(FK_UserId, Salt, HashedPassword, Expires, Expiration)
		VALUES
		(@UserId, @Salt, @HashedPassword, @Expires, @Expiration)
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