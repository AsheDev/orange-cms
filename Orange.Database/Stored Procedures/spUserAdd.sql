USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserAdd') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserAdd
GO
CREATE PROCEDURE o.UserAdd
	@Name NVARCHAR(256),
	@Email NVARCHAR(256),
	@RoleId INT,
	@UserId INT,
	@CallingUserId INT -- if you're impersonating should you be allowed to modify the settings?
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		DECLARE @NewUserId INT;
		---
		IF EXISTS (SELECT Id FROM o.Users WHERE Email = @Email)
		BEGIN
			SET @NewUserId = (SELECT Id FROM o.Users WHERE Email = @Email);
			---
			UPDATE o.Users
			SET IsActive = 1
			WHERE Id = @NewUserId;
		END
		ELSE 
		BEGIN
			INSERT INTO o.Users
			(Name, Email, FK_RoleId)
			VALUES
			(@Name, @Email, @RoleId);
			---
			SET @NewUserId = SCOPE_IDENTITY();
			---
		END
		---
		EXEC o.UserGet @UserId = @NewUserId;
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