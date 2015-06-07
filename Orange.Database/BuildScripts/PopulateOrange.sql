USE DevOrange
GO

INSERT INTO o.[Permissions]
(Name, IsRemovable, IsHidden)
VALUES
('Orange', 0, 1),
('Admin', 0, 0),
('Basic', 1, 0);
---
INSERT INTO o.Users
(Name, Email, IsVisible, FK_PermissionId)
VALUES
('Orange', 'orange@michaelovies.com', 0, 1),
('AdminUser', 'admin@admin.com', 1, 2),
('BasicUser', 'basic@basic.com', 1, 3);
---
INSERT INTO o.Accessibility
(FK_PermissionId, ManagePosts, CreateNewUsers, AccessSettings, CanImpersonate, ViewMetrics)
VALUES
(1, 1, 1, 1, 1, 1),
(2, 1, 1, 1, 1, 1),
(3, 1, 0, 0, 0, 0);
---
INSERT INTO o.Posts
(FK_UserId, [Subject], Body, Created, EffectiveDate)
VALUES
(2, 'My First Post!', 'The body and the blood, baby!', GETUTCDATE(), '2020-04-26 04:00:00'),
(2, 'This isn''t even my final form.', 'I don''t really know what else to say honestly.', DATEADD(HOUR, 2, GETUTCDATE()), DATEADD(HOUR, 2, GETUTCDATE())),
(2, 'Okay, let''s do this.', 'I''m literally done here!', DATEADD(HOUR, 3, GETUTCDATE()), DATEADD(HOUR, 4, GETUTCDATE()));
--- TODO: how do I know where to fit the link in the LinkText field?
--INSERT INTO o.Links
--(FK_CreatedByUserId, Title, Body, LinkText, Url, IsVisible)
--VALUES
--(2, 'Every kid needs a champion', 'Rita Pierson, a teacher for 40 years, once heard a colleague say, "They don''t pay me to like the kids." Her response: 
--"Kids don''t learn from people they don’t like.’” A rousing call to educators to believe in their students and actually 
--connect with them on a real, human, personal level.', 'Click here to watch', 'https://www.ted.com/talks/rita_pierson_every_kid_needs_a_champion?language=en', 1);
---
INSERT INTO o.PasswordSettings
(MaxPasswordAttempts, ExpirationInDays, ResetExpirationInMinutes)
VALUES
(3, 90, 900);
---
INSERT INTO o.EditTypes
(Name)
VALUES
('Created'),
('Modified'),
('Removed');
--- this password is !Orange_2015!
INSERT INTO o.Passwords
(FK_UserId, Salt, HashedPassword, Attempts, Expires, Expiration, IsLocked)
VALUES
(1, 'SdzZONlArQhcIbLpvmV2HwGZME4fdD63', '50000:SdzZONlArQhcIbLpvmV2HwGZME4fdD63:+1fu728N7y68tGqVlxt7qc8XIt+gvp2R', 0, 0, '2020-05-16 02:19:27.217', 0);
---
INSERT INTO o.Pages
(Name, [Description], URL, IsPublic, IsActive)
VALUES
('Home', '', '/Home', 1, 1),
('About', '', '/About', 1, 1),
('Contact', '', '/Contact', 1, 1),
('Peel', '', '/Peel', 1, 1), -- this is the access portal
('Admin', '', '/Admin', 0, 1);
---
EXEC o.RecordNavigation @UserId = 1, @CallingUserId = 1, @PageId = 1
---
EXEC o.RecordAccess @UserId = 1, @CallingUserId = 1, @Action = 1, @Success = 1, @OperatingSystem = 'Windows!', @IPAddress = '192.168.1.1'
---
INSERT INTO o.TableListing(Name, [Description])
SELECT Name, ''
FROM sys.tables