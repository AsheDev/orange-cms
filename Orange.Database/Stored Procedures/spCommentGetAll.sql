USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'CommentGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.CommentGetAll
GO
CREATE PROCEDURE o.CommentGetAll
	@PostId INT
--WITH ENCRYPTION AS
AS
	SET NOCOUNT ON;
	---
	SELECT Id, FK_PostId, FK_UserId, ProvidedName, Body, Created, ApprovalDate, Approval, IsActive 
	FROM o.PostComments
	WHERE FK_PostId = @PostId;
GO