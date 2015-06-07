USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserToggleInSystem') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserToggleInSystem
GO
CREATE PROCEDURE o.UserToggleInSystem
	@UserId INT = NULL,
	@Email NVARCHAR(256) = NULL
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		DECLARE @CurrentUserId INT;
		---
		IF(@UserId IS NULL)
		BEGIN
			UPDATE o.Users
			SET InSystem = CASE WHEN InSystem = 0 THEN 1 ELSE 0 END
			WHERE Email = @Email;
			---
			SET @CurrentUserId = (SELECT Id FROM o.Users WHERE Email = @Email);
		END
		ELSE
		BEGIN
			UPDATE o.Users
			SET InSystem = CASE WHEN InSystem = 0 THEN 1 ELSE 0 END
			WHERE Id = @UserId;
			---
			SET @CurrentUserId = @UserId;
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