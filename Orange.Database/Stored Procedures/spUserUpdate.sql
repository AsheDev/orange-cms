USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserUpdate
GO
CREATE PROCEDURE o.UserUpdate
	@UserId INT, -- the user making the change
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@Id INT, -- the userr being changed
	@Name NVARCHAR(256),
	@Email NVARCHAR(256),
	@RoleId INT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.Users
		SET Name = @Name,
			Email = @Email,
			FK_RoleId = @RoleId
		WHERE Id = @Id;
		---
		EXEC o.UserGet @UserId = @Id;
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