USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostGet
GO
CREATE PROCEDURE o.PostGet
	@PostId INT
AS
	SET NOCOUNT ON;
	---
	SELECT P.Id, P.FK_UserId, [Subject], P.Body, P.Created, EffectiveDate, 
		(SELECT COUNT(Id) FROM o.PostComments WHERE FK_PostId = @PostId) AS CommentCount, 
		IsPubliclyVisible, P.IsActive
	FROM o.Posts AS P
	WHERE P.Id = @PostId;
	---
	SELECT T.Id, T.Name, T.IsActive
	FROM o.TagMap AS TM
	INNER JOIN o.Posts AS P
	ON P.Id = TM.FK_PostId
	INNER JOIN o.Tags AS T
	ON T.Id = TM.FK_TagId
	WHERE P.Id = @PostId;
GO