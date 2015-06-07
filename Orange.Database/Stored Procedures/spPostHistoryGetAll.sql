USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PostHistoryGetAll') AND (TYPE = 'P')))
	DROP PROCEDURE o.PostHistoryGetAll
GO
CREATE PROCEDURE o.PostHistoryGetAll
	@PostId INT
AS
	SET NOCOUNT ON;
	---
	IF(@PostId <= 0)
	BEGIN
		SELECT FK_PostId, FK_EditTypeId, FK_UserId, [TimeStamp], [Subject], Body, Created, EffectiveDate, 
			   IsPubliclyVisible, FK_CallerId, IsActive
		FROM o.PostEditHistory;
	END
	ELSE
	BEGIN
		SELECT FK_PostId, FK_EditTypeId, FK_UserId, [TimeStamp], [Subject], Body, Created, EffectiveDate, 
			   IsPubliclyVisible, FK_CallerId, IsActive
		FROM o.PostEditHistory
		WHERE FK_PostId = 4;
	END	
GO