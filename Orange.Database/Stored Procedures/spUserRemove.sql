USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserRemove') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserRemove
GO
CREATE PROCEDURE o.UserRemove
	@UserId INT,
	@CallingUserId INT -- if you're impersonating should you be allowed to modify the settings?
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.Users
			SET IsActive = 0
		WHERE Id = @UserId;
		---
		SELECT CAST(1 AS BIT) -- SUCCESS
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
		SELECT CAST(0 AS BIT) -- FAILURE
	END CATCH;
GO