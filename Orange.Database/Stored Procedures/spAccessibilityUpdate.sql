USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'AccessibilityUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.AccessibilityUpdate
GO
CREATE PROCEDURE o.AccessibilityUpdate
	@PermissionId INT,
	@ManagePosts BIT,
	@CreateNewUsers BIT,
	@AccessSettings BIT,
	@CanImpersonate BIT,
	@ViewMetrics BIT,
	@IsActive BIT,
	@UserId INT,
	@CallingUserId INT -- if you're impersonating should you be allowed to modify the settings?
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.Accessibility
		SET ManagePosts = @ManagePosts,
			CreateNewUsers = @CreateNewUsers,
			AccessSettings = @AccessSettings,
			CanImpersonate = @CanImpersonate,
			ViewMetrics = @ViewMetrics,
			IsActive = @IsActive
		WHERE FK_PermissionId = @PermissionId;
		---
		EXEC o.AccessibilityGet @PermissionId = @PermissionId;
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
		EXEC o.AccessibilityGet @PermissionId = -99;
	END CATCH;
GO