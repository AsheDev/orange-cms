USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostGetLatest') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostGetLatest
GO
CREATE PROCEDURE o.PostGetLatest
AS
	SET NOCOUNT ON;
	---
	SELECT TOP(1) Id, FK_UserId, [Subject], Body, Created, EffectiveDate, 
		(SELECT COUNT(Id) FROM o.PostComments WHERE FK_PostId = Id) AS CommentCount,
		IsPubliclyVisible, IsActive 
	FROM o.Posts
	WHERE IsActive = 1
	AND IsPubliclyVisible = 1
	ORDER BY EffectiveDate ASC;
	---
	SELECT T.Id, T.Name, T.IsActive
	FROM o.TagMap AS TM
	INNER JOIN o.Posts AS P
	ON P.Id = TM.FK_PostId
	INNER JOIN o.Tags AS T
	ON T.Id = TM.FK_TagId
	WHERE P.IsActive = 1
	AND P.IsPubliclyVisible = 1
	ORDER BY P.EffectiveDate ASC;
GO