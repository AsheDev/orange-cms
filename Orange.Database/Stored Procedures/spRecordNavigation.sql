USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'RecordNavigation') AND (TYPE = 'P')))
	DROP PROCEDURE o.RecordNavigation
GO
CREATE PROCEDURE o.RecordNavigation
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@PageId INT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		INSERT INTO o.NavigationHistory
		(FK_UserId, FK_PageId, [TimeStamp])
		VALUES
		(@UserId, @PageId, GETUTCDATE());
		---
		DECLARE @NavId INT = SCOPE_IDENTITY();
		---
		EXEC o.RecordNavigationGet @NavId = @NavId;
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
		EXEC o.RecordNavigationGet @NavId = -99;
	END CATCH;
GO