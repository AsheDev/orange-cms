USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PermissionUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.PermissionUpdate
GO
CREATE PROCEDURE o.PermissionUpdate
	@RoleId INT,
	@ManagePosts BIT,
	@ManagePostComments BIT,
	@CanComment BIT,
	@ManageUsers BIT,
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
		UPDATE o.[Permissions]
		SET ManagePosts = @ManagePosts,
			ManagePostComments = @ManagePostComments,
			CanComment = @CanComment,
			ManageUsers = @ManageUsers,
			AccessSettings = @AccessSettings,
			CanImpersonate = @CanImpersonate,
			ViewMetrics = @ViewMetrics,
			IsActive = @IsActive
		WHERE FK_RoleId = @RoleId;
		---
		EXEC o.PermissionGet @RoleId = @RoleId;
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
		EXEC o.PermissionGet @RoleId = -99;
	END CATCH;
GO