USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserGetAll
GO
CREATE PROCEDURE o.UserGetAll
AS
	SET NOCOUNT ON;
	---
	SELECT Id, Name, Email, IsVisible, FK_PermissionId, InSystem, IsActive
	FROM o.Users;
GO