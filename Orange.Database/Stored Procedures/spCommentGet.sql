USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentGet
GO
CREATE PROCEDURE o.CommentGet
	@CommentId INT
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	---
	SELECT Id, FK_PostId, FK_UserId, ProvidedName, Body, Created, ApprovalDate, Approval, IsActive 
	FROM o.PostComments
	WHERE Id = @CommentId;
GO