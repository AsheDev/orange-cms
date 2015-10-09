USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentGetChildren') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentGetChildren
GO
CREATE PROCEDURE o.CommentGetChildren
	@CommentId INT
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	---
	SELECT Id, FK_PostId, FK_UserId, ProvidedName, Body, Created, ApprovalDate, Approval, EditKey, TopLevel,
		CASE 
			WHEN (SELECT COUNT(FK_ReplyId) FROM o.PostCommentReplyMap WHERE FK_CommentId = PC.Id) > 0 THEN (SELECT COUNT(FK_ReplyId) FROM o.PostCommentReplyMap WHERE FK_CommentId = PC.Id)
			ELSE 0
		END AS ReplyCount, IsActive
	FROM o.PostCommentReplyMap AS R
	INNER JOIN o.PostComments AS PC
	ON PC.Id = R.FK_ReplyId
	WHERE R.FK_CommentId = @CommentId
	AND IsActive = 1;
GO