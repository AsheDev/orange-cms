USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostRemove') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostRemove
GO
CREATE PROCEDURE o.PostRemove
	@PostId INT,
	@UserId INT,
	@CallingUserId INT -- if you're impersonating should you be allowed to modify the settings?
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.Posts
			SET IsActive = 0
		WHERE Id = @PostId;
		---
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Removed');
		DECLARE @Created DATETIME = (SELECT Created FROM o.Posts WHERE Id = @PostId);
		INSERT INTO o.PostEditHistory
		(FK_PostId, FK_EditTypeId, [TimeStamp], FK_UserId, [Subject], Body, Created, EffectiveDate, IsPubliclyVisible, FK_CallerId, IsActive)
		SELECT TOP 1 FK_PostId, @EditTypeId, GETUTCDATE(), FK_UserId, [Subject], Body, @Created, EffectiveDate, IsPubliclyVisible, @CallingUserId, 0
		FROM o.PostEditHistory
		ORDER BY [TimeStamp] DESC;
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