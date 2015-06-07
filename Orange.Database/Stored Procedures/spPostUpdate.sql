USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostUpdate
GO
CREATE PROCEDURE o.PostUpdate
	@PostId INT, -- TODO: this will be used for update histories
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@Subject NVARCHAR(256),
	@Body NVARCHAR(MAX),
	@EffectiveDate DATETIME,
	@IsPubliclyVisible BIT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.Posts
		SET [Subject] = @Subject,
			Body = @Body,
			EffectiveDate = @EffectiveDate,
			IsPubliclyVisible = @IsPubliclyVisible
		WHERE Id = @PostId;
		---
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Modified');
		DECLARE @Created DATETIME = (SELECT Created FROM o.Posts WHERE Id = @PostId);
		INSERT INTO o.PostEditHistory
		(FK_PostId, FK_EditTypeId, [TimeStamp], FK_UserId, [Subject], Body, Created, EffectiveDate, IsPubliclyVisible, FK_CallerId)
		VALUES
		(@PostId, @EditTypeId, GETUTCDATE(), @UserId, @Subject, @Body, @Created, @EffectiveDate, @IsPubliclyVisible, @CallingUserId);
		---
		EXEC o.PostGet @PostId = @PostId;
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
		EXEC o.PostGet @PostId = -99;
	END CATCH;
GO