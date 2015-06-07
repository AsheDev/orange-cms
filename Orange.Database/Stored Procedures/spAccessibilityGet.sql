USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'AccessibilityGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.AccessibilityGet
GO
CREATE PROCEDURE o.AccessibilityGet
	@PermissionId INT
AS
	SET NOCOUNT ON;
	---
	SELECT FK_PermissionId, ManagePosts, CreateNewUsers, AccessSettings, CanImpersonate, ViewMetrics, IsActive 
	FROM o.Accessibility
	WHERE FK_PermissionId = @PermissionId;
GO