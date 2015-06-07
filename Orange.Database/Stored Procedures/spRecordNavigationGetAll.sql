USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'RecordNavigationGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.RecordNavigationGetAll
GO
CREATE PROCEDURE o.RecordNavigationGetAll
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	---
	SELECT NH.Id, FK_UserId, [TimeStamp], P.Id, Name, [Description], URL, IsPublic, IsActive
	FROM o.Pages AS P
	INNER JOIN o.NavigationHistory AS NH
	ON NH.FK_PageId = P.Id;
GO