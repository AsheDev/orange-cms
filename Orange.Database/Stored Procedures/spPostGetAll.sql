USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostGetAll
GO
CREATE PROCEDURE o.PostGetAll
AS
	SET NOCOUNT ON;
	---
	SELECT P.Id, P.FK_UserId, P.[Subject], P.Body, P.Created, P.EffectiveDate, 
		COUNT(PC.Id) AS CommentCount,
		P.IsPubliclyVisible, P.IsActive 
	FROM o.Posts AS P
	FULL OUTER JOIN o.PostComments AS PC ON PC.FK_PostId = P.Id
	GROUP BY P.Id, P.FK_UserId, P.[Subject], P.Body, P.Created, P.EffectiveDate, P.IsPubliclyVisible, P.IsActive;
GO