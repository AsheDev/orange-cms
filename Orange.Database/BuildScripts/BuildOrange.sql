USE MASTER
ALTER DATABASE DevOrange SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
DROP DATABASE DevOrange
GO
CREATE DATABASE DevOrange
GO
USE DevOrange
GO
CREATE SCHEMA [o]
GO
SET NOCOUNT ON
GO
-- needed to manually create login "Orange_Engine" with password 1234
CREATE USER Orange_Engine FOR LOGIN Orange_Engine
GO
ALTER USER Orange_Engine WITH DEFAULT_SCHEMA = [o]
GO
ALTER ROLE [db_owner] ADD MEMBER [Orange_Engine] -- set Orange_Engine as db_owner

IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Settings') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.Settings (
		ShowLoginButton				BIT DEFAULT 0,
		InactivityTimer				INT DEFAULT 3600 -- after an hour of inactivity the user is logged out of the system by a job
	);
END
GO

-- Handled by: PermissionOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Permissions') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.[Permissions] (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		Name						NVARCHAR(256) NOT NULL, -- Admin
		IsRemovable					BIT DEFAULT 1, -- the hidden account won't be removable (this may be a wildly bad idea)
		IsHidden					BIT DEFAULT 0,
		IsActive					BIT DEFAULT 1
	);
END
GO

-- for example, can the user make a new blog post? or delete posts? or something...
-- TODO: is this implementation too rigid?
-- Handled by: AccessibilityOps.cs (TODO: move to PermissionOps.cs - it makes more sense I think)
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Accessibility') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.Accessibility (
		FK_PermissionId				INT FOREIGN KEY REFERENCES o.[Permissions],
		ManagePosts					BIT DEFAULT 0,
		CreateNewUsers				BIT DEFAULT 0,
		AccessSettings				BIT DEFAULT 0,
		CanImpersonate				BIT DEFAULT 0,
		ViewMetrics					BIT DEFAULT 0,
		IsActive					BIT DEFAULT 1
	);
END
GO

-- Handled by: UserOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Users') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.Users (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		Name						NVARCHAR(256) NOT NULL,
		Email						NVARCHAR(256) NOT NULL,
		IsVisible					BIT DEFAULT 1, -- I want to hide my user account
		FK_PermissionId				INT FOREIGN KEY REFERENCES o.[Permissions](Id),
		InSystem					BIT DEFAULT 0,
		IsActive					BIT DEFAULT 1
	);
END
GO

-- Handled by: PasswordOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Passwords') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.Passwords (
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id),
		Salt						NVARCHAR(128),
		HashedPassword				NVARCHAR(128),
		Attempts					TINYINT DEFAULT 0,
		Expires						BIT DEFAULT 1,
		Expiration					DATETIME NOT NULL,
		IsLocked					BIT DEFAULT 0
	);
END
GO

-- Handled by: TODO
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'PasswordHistory') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.PasswordHistory (
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id),
		Salt						NVARCHAR(128),
		HashedPassword				NVARCHAR(128)
	);
END
GO

-- Handled by: PasswordOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'PasswordResetDetails') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.PasswordResetDetails (
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id), -- is this correct?
		AuthenticationUrl			NVARCHAR(128),
		Created						DATETIME,
		Expires						DATETIME
	);
END
GO

-- Handled by: PostOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Posts') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.Posts (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id),
		[Subject]					NVARCHAR(256) NOT NULL,
		Body						NVARCHAR(MAX) NOT NULL,
		Created						DATETIME NOT NULL, -- when the posts was actually created (could be the same as EffectiveDate if posted immediately)
		EffectiveDate				DATETIME NOT NULL, -- when the post will show up
		IsPubliclyVisible			BIT DEFAULT 1, -- maybe not everything should be on display?
		IsActive					BIT DEFAULT 1
	);
END
GO

IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'EditTypes') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.EditTypes (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		Name						NVARCHAR(256) NOT NULL -- Created, Updated, Removed
	);
END
GO

-- Handled by: UserOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'PostEditHistory') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.PostEditHistory (
		FK_PostId					INT FOREIGN KEY REFERENCES o.Posts(Id),
		FK_EditTypeId				INT FOREIGN KEY REFERENCES o.EditTypes(Id),
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id),
		[TimeStamp]					DATETIME, -- when the edit was made
		[Subject]					NVARCHAR(256),
		Body						NVARCHAR(MAX) NOT NULL,
		Created						DATETIME NOT NULL, -- when the posts was actually created (could be the same as EffectiveDate if posted immediately)
		EffectiveDate				DATETIME NOT NULL, -- when the post will show up
		IsPubliclyVisible			BIT DEFAULT 1, -- maybe not everything should be on display?
		FK_CallerId					INT FOREIGN KEY REFERENCES o.Users(Id), -- in case we're impersonating
		IsActive					BIT DEFAULT 1
	);
END
GO

--*** I think I may want this to be an addon ***--
--IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Links') AND (TYPE = 'U')))
--BEGIN
--	CREATE TABLE o.Links (
--		Id							INT IDENTITY(1,1) PRIMARY KEY,
--		FK_CreatedByUserId			INT FOREIGN KEY REFERENCES o.Users(Id),
--		Title						NVARCHAR(256) NOT NULL,
--		Body						NVARCHAR(MAX) NOT NULL,
--		LinkText					NVARCHAR(256) NOT NULL, -- for example "Click HERE to watch"
--		Url							NVARCHAR(256) NOT NULL,
--		IsVisible					BIT DEFAULT 1,
--		IsActive					BIT DEFAULT 1
--	);
--END
--GO

-- Handled by: PasswordSettingsOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'PasswordSettings') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.PasswordSettings (
		MaxPasswordAttempts			INT DEFAULT 3, -- max attempts before a user account gets locked (0 means no lockout)
		ExpirationInDays			INT DEFAULT 90, -- 0 would mean a password never expires
		ResetExpirationInMinutes	INT DEFAULT 900 -- how quickly a password reset link will expire
	);
END
GO

-- Handled by: TODO
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'Pages') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.Pages (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		Name						NVARCHAR(256) NOT NULL,
		[Description]				NVARCHAR(256),
		URL							NVARCHAR(256),
		IsPublic					BIT DEFAULT 1, -- if it's not public, how do I handle permission to view?
		IsActive					BIT DEFAULT 1
	);
END
GO

-- Handled by: (TODO) PermissionOPs.cs
-- If Page is not public, have a list of values of who can access it
-- If Page is public the list will just be an empty list
-- Admin (and Orange) always has access to private pages
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'PagesKey') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.PagesKey (
		FK_PageId					INT FOREIGN KEY REFERENCES o.Pages(Id),
		FK_PermissionId				INT FOREIGN KEY REFERENCES o.[Permissions](Id)
	);
END
GO
-- TODO: if not public, how do I want to handle permissions?

-- Handled by: MetricsOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'NavigationHistory') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.NavigationHistory (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id),
		FK_PageId					INT FOREIGN KEY REFERENCES o.Pages(Id),
		[TimeStamp]					DATETIME NOT NULL
	);
END
GO

-- Handled by: MetricsOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'AccessHistory') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.AccessHistory (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id),
		[Action]					BIT NOT NULL, -- 1 login, 0 logout
		Success						BIT NOT NULL, -- 1 successful, 0 unsuccessful (was the login attempt successful or not)
		[TimeStamp]					DATETIME NOT NULL
	);
END
GO

-- Handled by: MetricsOps.cs
IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'AccessHistoryDetails') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.AccessHistoryDetails (
		FK_AccessId					INT FOREIGN KEY REFERENCES o.AccessHistory(Id),
		OperatingSystem				NVARCHAR(256),
		IPAddress					NVARCHAR(256)
	);
END
GO

IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'TableListing') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.TableListing (
		Id							INT IDENTITY(1,1) PRIMARY KEY,
		Name						NVARCHAR(256) NOT NULL,
		[Description]				NVARCHAR(256) NOT NULL
	);
END
GO

IF (NOT EXISTS (SELECT * FROM sysobjects WHERE (name = N'ChangeLog') AND (TYPE = 'U')))
BEGIN
	CREATE TABLE o.ChangeLog (
		FK_TableId					INT FOREIGN KEY REFERENCES o.TableListing(Id), -- the table that was changed
		FK_UserId					INT FOREIGN KEY REFERENCES o.Users(Id), -- the user making the change
		FK_CallingUserId			INT FOREIGN KEY REFERENCES o.Users(Id), -- the REAL user making the change (possibly)
		Operation					TINYINT NOT NULL -- this is tied to an enum
	);
END
GO

-- generate the necessary stored procedures
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spLogIn.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spLogOut.sql

:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserGetByUsername.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserGetAll.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserAdd.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserUpdate.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserRemove.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spUserToggleInSystem.sql

:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordAdd.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordUpdate.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordRecordAttempt.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordResetGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordReset.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordSettingsGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPasswordSettingsUpdate.sql

:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPostGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPostGetAll.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPostAdd.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPostUpdate.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPostRemove.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spPostHistoryGetAll.sql

:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spRecordAccessGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spRecordAccessGetAll.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spRecordAccess.sql

:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spRecordNavigationGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spRecordNavigationGetAll.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spRecordNavigation.sql

:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spAccessibilityGet.sql
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\"Stored Procedures"\spAccessibilityUpdate.sql



-- populate the database with some basic data for testing purposes
:r C:\Users\Michael\Source\Workspaces\"Orange CMS"\Orange.Core\Orange.Database\BuildScripts\PopulateOrange.sql