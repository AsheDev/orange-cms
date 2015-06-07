USE DevOrange
GO
IF (EXISTS (SELECT name FROM sysobjects WHERE (name = N'PasswordGet') AND (TYPE = 'P')))
	DROP PROCEDURE o.PasswordGet
GO
CREATE PROCEDURE o.PasswordGet
	@UserId INT = NULL,
	@username NVARCHAR(256) = NULL
AS
	-- WITH ENCRYPTION ON AS
	SET NOCOUNT ON;
	IF(@UserId IS NULL)
	BEGIN
		SELECT FK_UserId, Salt, HashedPassword, Attempts, Expires, Expiration, IsLocked
		FROM o.Passwords AS P
		INNER JOIN o.Users AS U
		ON U.Id = P.FK_UserId
		WHERE U.Email = @username
		OR U.Name = @username;
	END
	ELSE
	BEGIN
		SELECT FK_UserId, Salt, HashedPassword, Attempts, Expires, Expiration, IsLocked
		FROM o.Passwords
		WHERE FK_UserId = @UserId;
	END
GO