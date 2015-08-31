USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostGetLatest') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostGetLatest
GO
CREATE PROCEDURE o.PostGetLatest
AS
	SET NOCOUNT ON;
	---
	DECLARE @PostId INT = (SELECT TOP(1) Id
	FROM o.Posts
	WHERE IsPubliclyVisible = 1
	AND IsActive = 1
	ORDER BY EffectiveDate DESC);
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