USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PermissionGetByUserId') AND (TYPE = 'P')))
	DROP PROCEDURE o.PermissionGetByUserId
GO
CREATE PROCEDURE o.PermissionGetByUserId
	@UserId INT
AS
	SET NOCOUNT ON;
	---
	SELECT P.FK_RoleId, P.ManagePosts, P.ManagePostComments, P.CanComment, P.ManageUsers, P.AccessSettings, 
		P.CanImpersonate, P.ViewMetrics, P.IsActive 
	FROM o.[Permissions] AS P
	INNER JOIN o.Users AS U
	ON U.FK_RoleId = P.FK_RoleId
	WHERE U.Id = @UserId;
GO