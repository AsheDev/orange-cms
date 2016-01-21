USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'UserGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.UserGetAll
GO
CREATE PROCEDURE o.UserGetAll
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
	FROM o.Users;
GO