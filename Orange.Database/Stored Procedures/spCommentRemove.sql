USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentRemove') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentRemove
GO
CREATE PROCEDURE o.CommentRemove
	@UserId INT,
	@CallingUserId INT, -- if you're impersonating should you be allowed to modify the settings?
	@CommentId INT
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		UPDATE o.PostComments
			SET IsActive = 0
		WHERE Id = @CommentId;
		---
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Removed');
		DECLARE @Created DATETIME = (SELECT Created FROM o.PostComments WHERE Id = @CommentId);
		---
		INSERT INTO o.PostCommentEditHistory
		(FK_CommentId, FK_EditTypeId, FK_UserId, [TimeStamp], ProvidedName, Body, Created, ApprovalDate, Approval, FK_CallerId, IsActive)
		SELECT TOP 1 FK_CommentId, @EditTypeId, @UserId, GETUTCDATE(), ProvidedName, Body, Created, ApprovalDate, Approval, @CallingUserId, 0
		FROM o.PostCommentEditHistory
		ORDER BY [TimeStamp] DESC;
		---
		SELECT CAST(1 AS BIT) -- SUCCESS
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
		SELECT CAST(0 AS BIT) -- FAILURE
	END CATCH;
GO