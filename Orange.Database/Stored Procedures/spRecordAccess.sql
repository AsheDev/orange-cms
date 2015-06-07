USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'RecordAccess') AND (TYPE = 'P')))
	DROP PROCEDURE o.RecordAccess
GO
CREATE PROCEDURE o.RecordAccess
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@Action BIT,
	@Success BIT,
	@OperatingSystem NVARCHAR(256),
	@IPAddress	NVARCHAR(256)
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		INSERT INTO o.AccessHistory
		(FK_UserId, [Action], Success, [TimeStamp])
		VALUES
		(@UserId, @Action, @Success, GETUTCDATE());
		---
		DECLARE @AccessId INT = SCOPE_IDENTITY();
		---
		INSERT INTO o.AccessHistoryDetails
		(FK_AccessId, OperatingSystem, IPAddress)
		VALUES
		(@AccessId, @OperatingSystem, @IPAddress);
		---
		EXEC o.RecordAccessGet @AccessId = @AccessId;
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
		EXEC o.RecordAccessGet @AccessId = -99;
	END CATCH;
GO