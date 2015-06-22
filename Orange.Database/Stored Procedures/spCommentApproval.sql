USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentApproval') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentApproval
GO
CREATE PROCEDURE o.CommentApproval
	@UserId INT,
	@CallingUserId INT,
	@CommentId INT,
	@Approval TINYINT
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	SET XACT_ABORT ON;
	BEGIN TRY
		---
		DECLARE @EditTypeId INT = (SELECT Id FROM o.EditTypes WHERE Name = 'Modified');
		DECLARE @Timestamp DATETIME = GETUTCDATE();
		---
		UPDATE o.PostComments
		SET Approval = @Approval,
			ApprovalDate = @Timestamp
		WHERE Id = @CommentId;
		---
		INSERT INTO o.PostCommentEditHistory
		(FK_CommentId, FK_EditTypeId, FK_UserId, [TimeStamp], ProvidedName, Body, Created, ApprovalDate, Approval, FK_CallerId)
		SELECT TOP 1 @CommentId, @EditTypeId, @UserId, @Timestamp, ProvidedName, Body, Created, @Timestamp, @Approval, @CallingUserId
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
		EXEC o.CommentGet @CommetId = -99;
	END CATCH;
GO