USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserGetByUsername') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserGetByUsername
GO
CREATE PROCEDURE o.UserGetByUsername
	@Username NVARCHAR(256)
AS
	SET NOCOUNT ON;
	---
	SELECT Id, Name, Email, IsVisible, FK_PermissionId, InSystem, IsActive
	FROM o.Users
	WHERE Email = @Username
	OR Name = @Username;
GO