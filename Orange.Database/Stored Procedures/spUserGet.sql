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
	SELECT Id, ObfuscatedId, 
		CASE
			WHEN EXISTS (SELECT FK_ImpersonatedId FROM o.UserImpersonationMap WHERE FK_UserId = Id) 
				THEN (SELECT FK_ImpersonatedId FROM o.UserImpersonationMap WHERE FK_UserId = Id)
			ELSE 0
		END AS FK_ImpersonatingId,
		 Name, Email, IsVisible, FK_RoleId, InSystem, IsActive
	FROM o.Users
	WHERE Id = @UserId;
GO