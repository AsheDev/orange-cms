USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'RecordAccessGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.RecordAccessGetAll
GO
CREATE PROCEDURE o.RecordAccessGetAll
AS
	SET NOCOUNT ON;
	---
	SELECT Id, FK_UserId, [Action], Success, [TimeStamp], OperatingSystem, IPAddress
	FROM o.AccessHistory AS AH
	INNER JOIN o.AccessHistoryDetails AS AHD
	ON AHD.FK_AccessId = AH.Id;
GO