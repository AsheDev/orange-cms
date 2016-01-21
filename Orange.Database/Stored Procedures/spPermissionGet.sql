USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PermissionsGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.PermissionsGet
GO
CREATE PROCEDURE o.PermissionGet
	@RoleId INT
AS
	SET NOCOUNT ON;
	---
	SELECT FK_RoleId, ManagePosts, ManagePostComments, CanComment, ManageUsers, AccessSettings, 
		CanImpersonate, ViewMetrics, IsActive 
	FROM o.[Permissions]
	WHERE FK_RoleId = @RoleId;
GO