USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentUpdate') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentUpdate
GO
CREATE PROCEDURE o.CommentUpdate
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@CommentId INT,
	@ProvidedName NVARCHAR(255),
	@Body NVARCHAR(1200)
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.PostComments
		SET ProvidedName = @ProvidedName,
			Body = @Body
		WHERE Id = @CommentId;
		---
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Modified');
		DECLARE @Created DATETIME = (SELECT Created FROM o.PostComments WHERE Id = @CommentId); -- I can likely remove this
		DECLARE @Now DATETIME = GETUTCDATE();
		INSERT INTO o.PostCommentEditHistory
		(FK_CommentId, FK_EditTypeId, FK_UserId, [TimeStamp], ProvidedName, Body, Created, ApprovalDate, Approval, FK_CallerId)
		SELECT TOP 1 @CommentId, @EditTypeId, @UserId, @Now, @ProvidedName, @Body, @Created, @Now, Approval, @CallingUserId
		FROM o.PostCommentEditHistory
		ORDER BY [TimeStamp] DESC;
		---
		EXEC o.CommentGet @CommentId = @CommentId;
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