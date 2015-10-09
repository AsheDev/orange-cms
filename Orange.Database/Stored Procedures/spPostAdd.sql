USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostAdd') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostAdd
GO
CREATE PROCEDURE o.PostAdd
	@UserId INT,
	@CallingUserId INT,
	@Subject NVARCHAR(256),
	@Body NVARCHAR(MAX),
	@EffectiveDate DATETIME,
	@IsPubliclyVisible BIT,
	@Tags PostTags READONLY
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
		--- TEMPORARY 10/6/2015 - this isn't working and it's fucking with my other testing right now
		--IF((SELECT COUNT(Name) FROM @Tags) > 0)
		--BEGIN
		--	- this should avoid duplicates being saved
		--	INSERT INTO o.Tags (Name)
		--	SELECT Name FROM @Tags
		--	EXCEPT
		--	SELECT Name FROM o.Tags;
		--	-
		--	DECLARE @CurrentTagId INT;
		--	DECLARE @CurrentName NVARCHAR(256);
		--	WHILE EXISTS (SELECT Processed FROM @Tags WHERE Processed = 0)
		--	BEGIN
		--		SET @CurrentName = (SELECT TOP(1) Name FROM @Tags WHERE Processed = 0);
		--		SET @CurrentTagId = (SELECT Id FROM o.Tags WHERE Name = @CurrentName);
		--		-
		--		INSERT INTO o.TagMap
		--		(FK_PostId, FK_TagId)
		--		VALUES
		--		(@NewPostId, @CurrentTagId);
		--	END
		--END
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