USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostGetAll
GO
CREATE PROCEDURE o.PostGetAll
AS
	SET NOCOUNT ON;
	---
	SELECT Id, FK_UserId, [Subject], Body, Created, EffectiveDate, IsPubliclyVisible, IsActive 
	FROM o.Posts
GO