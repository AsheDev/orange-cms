USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'TagAdd') AND (TYPE = 'P')))
	DROP PROCEDURE o.TagAdd
GO
CREATE PROCEDURE o.TagAdd
	@Name NVARCHAR(256)
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		INSERT INTO o.Tags
		(Name)
		VALUES
		(@Name);
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
	END CATCH;
GO