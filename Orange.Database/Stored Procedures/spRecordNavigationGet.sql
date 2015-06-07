USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'RecordNavigationGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.RecordNavigationGet
GO
CREATE PROCEDURE o.RecordNavigationGet
	@NavId INT
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	---
	SELECT NH.Id, FK_UserId, [TimeStamp], P.Id, Name, [Description], URL, IsPublic, IsActive
	FROM o.Pages AS P
	INNER JOIN o.NavigationHistory AS NH
	ON NH.FK_PageId = P.Id
	WHERE NH.Id = @NavId;
GO