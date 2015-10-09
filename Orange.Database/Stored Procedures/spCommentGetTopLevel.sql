USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentGetTopLevel') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentGetTopLevel
GO
CREATE PROCEDURE o.CommentGetTopLevel
	@PostId INT
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	---
	SELECT Id, FK_PostId, FK_UserId, ProvidedName, Body, Created, ApprovalDate, Approval, EditKey, TopLevel,
		CASE 
			WHEN (SELECT COUNT(FK_ReplyId) FROM o.PostCommentReplyMap WHERE FK_CommentId = PC.Id) > 0 THEN (SELECT COUNT(FK_ReplyId) FROM o.PostCommentReplyMap WHERE FK_CommentId = PC.Id)
			ELSE 0 
		END AS ReplyCount, IsActive
	FROM o.PostComments AS PC
	WHERE PC.FK_PostId = @PostId
	AND PC.TopLevel = 1
	AND IsActive = 1;
GO