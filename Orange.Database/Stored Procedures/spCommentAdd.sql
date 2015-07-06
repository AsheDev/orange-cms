USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentAdd') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentAdd
GO
CREATE PROCEDURE o.CommentAdd
	@UserId INT,
	@CallingUserId INT,
	@PostId INT,
	@ProvidedName NVARCHAR(255),
	@Body NVARCHAR(1200),
	@EditKey NVARCHAR(7)
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Created');
		DECLARE @Created DATETIME = GETUTCDATE();
		---
		IF(@UserId = 0)
		BEGIN
			SET @UserId = (SELECT Id FROM o.Users WHERE Name = 'Anonymous');
			SET @CallingUserId = @UserId;
		END
		---
		INSERT INTO o.PostComments
		(FK_PostId, FK_UserId, ProvidedName, Body, Created, ApprovalDate, EditKey)
		VALUES
		(@PostId, @UserId, @ProvidedName, @Body, @Created, DATEADD(SECOND, -1, @Created), @EditKey);
		---
		DECLARE @NewCommentId INT = SCOPE_IDENTITY();
		---
		INSERT INTO o.PostCommentEditHistory
		(FK_CommentId, FK_EditTypeId, FK_UserId, [TimeStamp], ProvidedName, Body, Created, ApprovalDate, EditKey, FK_CallerId)
		VALUES
		(@NewCommentId, @EditTypeId, @UserId, @Created, @ProvidedName, @Body, @Created, DATEADD(SECOND, -1, @Created), @EditKey, @CallingUserId);
		---
		EXEC o.CommentGet @CommentId = @NewCommentId;
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
		EXEC o.CommentGet @CommetId = -99;
	END CATCH;
GO