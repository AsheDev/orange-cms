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
	SELECT Id, FK_UserId, [Subject], Body, Created, EffectiveDate, IsPubliclyVisible, IsActive 
	FROM o.Posts
	WHERE Id = @PostId;
GO