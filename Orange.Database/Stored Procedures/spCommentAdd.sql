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
	@EditKey NVARCHAR(7),
	@TopLevel BIT,
	@ParentCommentId INT = 0 -- top-level comments can be zeroed out
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
		--- if comments are auto-approved then set the approval flag to true
		DECLARE @Approval BIT = 0;
		IF(SELECT AwaitModeration FROM o.PostSettings) = 0
		BEGIN
			SET @Approval = 1;
		END
		---
		INSERT INTO o.PostComments
		(FK_PostId, FK_UserId, ProvidedName, Body, Created, ApprovalDate, EditKey, TopLevel, Approval)
		VALUES
		(@PostId, @UserId, @ProvidedName, @Body, @Created, DATEADD(SECOND, -1, @Created), @EditKey, @TopLevel, @Approval);
		---
		DECLARE @NewCommentId INT = SCOPE_IDENTITY();
		---
		IF(@ParentCommentId > 0)
		BEGIN
			INSERT INTO o.PostCommentReplyMap
			(FK_CommentId, FK_ReplyId)
			VALUES
			(@ParentCommentId, @NewCommentId);
		END
		---
		INSERT INTO o.PostCommentEditHistory
		(FK_CommentId, FK_EditTypeId, FK_UserId, [TimeStamp], ProvidedName, Body, Created, ApprovalDate, EditKey, FK_CallerId, TopLevel, Approval)
		VALUES
		(@NewCommentId, @EditTypeId, @UserId, @Created, @ProvidedName, @Body, @Created, DATEADD(SECOND, -1, @Created), @EditKey, @CallingUserId, @TopLevel, @Approval);
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
		EXEC o.CommentGet @CommentId = -99;
	END CATCH;
GO