USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserGet
GO
CREATE PROCEDURE o.UserGet
	@UserId INT
AS
	SET NOCOUNT ON;
	---
	SELECT Id, Name, Email, IsVisible, FK_PermissionId, InSystem, IsActive
	FROM o.Users
	WHERE Id = @UserId;
GO