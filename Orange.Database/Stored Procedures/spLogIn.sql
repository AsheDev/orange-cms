USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'LogIn') AND (TYPE = 'P')))
	DROP PROCEDURE o.[LogIn]
GO
CREATE PROCEDURE o.[LogIn]
	@Username NVARCHAR(256) = NULL,
	@UserId INT = NULL
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		DECLARE @CurrentUserId INT;
		---
		IF(@Username IS NULL)
		BEGIN
			UPDATE o.Users
			SET InSystem = 1
			WHERE Id = @UserId;
			---
			SET @CurrentUserId = @UserId;
		END
		ELSE
		BEGIN
			UPDATE o.Users
			SET InSystem = 1
			WHERE Email = @Username
			OR Name = @Username;
			---
			SET @CurrentUserId = (SELECT Id FROM o.Users WHERE Email = @Username OR Name = @Username);
		END
		---
		EXEC o.UserGet @UserId = @CurrentUserId;
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
		EXEC o.UserGet @UserId = -99;
	END CATCH;
GO