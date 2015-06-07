﻿USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostAdd') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostAdd
GO
CREATE PROCEDURE o.PostAdd
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
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Created');
		DECLARE @Created DATETIME = GETUTCDATE();
		IF(@EffectiveDate <= @Created)
		BEGIN
			SET @EffectiveDate = @Created;
		END
		---
		INSERT INTO o.Posts
		(FK_UserId, [Subject], Body, Created, EffectiveDate, IsPubliclyVisible)
		VALUES
		(@UserId, @Subject, @Body, @Created, @EffectiveDate, @IsPubliclyVisible);
		---
		DECLARE @NewPostId INT = SCOPE_IDENTITY();
		---
		INSERT INTO o.PostEditHistory
		(FK_PostId, FK_EditTypeId, [TimeStamp], FK_UserId, [Subject], Body, Created, EffectiveDate, IsPubliclyVisible, FK_CallerId)
		VALUES
		(@NewPostId, @EditTypeId, @Created, @UserId, @Subject, @Body, @Created, @EffectiveDate, @IsPubliclyVisible, @CallingUserId);
		---
		EXEC o.PostGet @PostId = @NewPostId;
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