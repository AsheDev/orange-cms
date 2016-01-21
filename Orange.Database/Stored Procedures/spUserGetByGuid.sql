USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserGetByGuid') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserGetByGuid
GO
CREATE PROCEDURE o.UserGetByGuid
	@ObfuscatedId UNIQUEIDENTIFIER
AS
	SET NOCOUNT ON;
	---
	SELECT Id, ObfuscatedId, 
		CASE
			WHEN EXISTS (SELECT FK_ImpersonatedId FROM o.UserImpersonationMap WHERE FK_UserId = Id) 
				THEN (SELECT FK_ImpersonatedId FROM o.UserImpersonationMap WHERE FK_UserId = Id)
			ELSE 0
		END AS FK_ImpersonatingId,
		 Name, Email, IsVisible, FK_RoleId, InSystem, IsActive
	FROM o.Users
	WHERE ObfuscatedId = @ObfuscatedId;
GO