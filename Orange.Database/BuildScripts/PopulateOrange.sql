--*****************************************************--
-- This is primarily intended for testing Stacy's site
--*****************************************************--

USE DevOrange
GO
---
INSERT INTO o.[Permissions]
(Name, IsRemovable, IsHidden)
VALUES
('Orange', 0, 1),
('Admin', 0, 0),
('Basic', 1, 0),
('Anonymous', 0, 1); -- people don't need to worry about this one
---
INSERT INTO o.Users
(Name, Email, IsVisible, FK_PermissionId)
VALUES
('Orange', 'orange@michaelovies.com', 0, 1),
('AdminUser', 'admin@admin.com', 1, 2),
('BasicUser', 'basic@basic.com', 1, 3),
('Anonymous', 'anon@anon.com', 0, 4); -- anonymous commenters
---
INSERT INTO o.Accessibility
(FK_PermissionId, ManagePosts, CreateNewUsers, AccessSettings, CanImpersonate, ViewMetrics)
VALUES
(1, 1, 1, 1, 1, 1),
(2, 1, 1, 1, 1, 1),
(3, 1, 0, 0, 0, 0),
(4, 0, 0, 0, 0, 0); -- the anonymous user
---
DECLARE @TwoHours DATETIME = DATEADD(HOUR, 2, GETUTCDATE())
DECLARE @FourHours DATETIME = DATEADD(HOUR, 4, GETUTCDATE())
EXEC o.PostAdd @UserId = 2, 
			   @Subject = 'My First Post!', 
			   @Body = '

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse in scelerisque elit. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Fusce scelerisque velit ipsum, at feugiat leo congue eget. In hac habitasse platea dictumst. Donec sem lacus, accumsan vitae malesuada eget, facilisis eget nibh. Morbi nibh metus, convallis ac blandit et, euismod vel ligula. Pellentesque ac felis mollis, auctor dolor eget, pharetra turpis. Morbi pharetra ante non volutpat facilisis.

Proin ipsum purus, tristique ut ipsum in, molestie aliquam ipsum. Nunc in ipsum lacinia lacus sodales semper. Aenean pretium, odio vitae dictum condimentum, mi orci cursus mi, eget varius felis diam sed ligula. In ex nibh, dapibus laoreet pretium non, mollis in tellus. Sed posuere dictum augue, vitae condimentum nulla maximus sed. Maecenas aliquam dolor et viverra convallis. Phasellus vitae diam tincidunt, lacinia diam eu, lobortis ante.

Curabitur tortor lorem, mollis quis vehicula vitae, vehicula rutrum urna. Duis pellentesque, nibh at ultrices facilisis, lacus urna accumsan ex, at tempor mi lorem in dui. Fusce augue mauris, dictum non volutpat nec, consequat vel justo. Mauris vestibulum quam nec fringilla consequat. Aliquam vehicula eu metus nec tincidunt. Nunc mollis mattis pharetra. Mauris placerat tellus eget ipsum pulvinar, quis eleifend lectus efficitur. ', 
			   @EffectiveDate = '2020-04-26 04:00:00', 
			   @CallingUserId = 2, 
			   @IsPubliclyVisible = 1
EXEC o.PostAdd @UserId = 2, 
			   @Subject = 'This isn''t even my final form.', 
			   @Body = '

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed cursus enim nec dolor aliquam suscipit. Phasellus efficitur ipsum nec euismod laoreet. Pellentesque semper est ante, id pharetra ante porta eu. Donec nec arcu at metus aliquam efficitur quis ut tellus. Nam eu urna elit. Etiam congue volutpat arcu quis tempus. Morbi tristique risus est, eget faucibus diam porta at.

Donec facilisis tellus magna, sit amet bibendum purus sollicitudin nec. Integer vestibulum sem nunc, pulvinar tristique nisl consectetur sit amet. Donec a felis lectus. Donec auctor mollis mauris, sed scelerisque velit ornare hendrerit. Etiam et augue ultrices, sodales purus a, auctor arcu. Sed arcu purus, suscipit at volutpat sed, maximus a elit. Sed nec purus ornare, sollicitudin leo semper, faucibus dolor. Interdum et malesuada fames ac ante ipsum primis in faucibus. Nullam dictum placerat arcu ac faucibus. Vestibulum sodales rhoncus erat, nec egestas metus maximus et. Maecenas sed sodales massa, vel ultricies turpis. Vivamus ut felis ac ipsum ultrices pharetra. Etiam massa ex, pretium vel velit sit amet, blandit porttitor purus. Duis scelerisque, est id ultrices vulputate, sem ligula imperdiet neque, ut convallis metus nunc sed dui. Pellentesque at vulputate ante, quis ullamcorper dui. Cras elementum sodales mauris et iaculis.

Donec tempus ante commodo ultricies faucibus. Nulla sed lectus nec felis aliquet dapibus. Pellentesque gravida risus lobortis arcu aliquam, vel dapibus odio mattis. Phasellus fermentum molestie ante, at ullamcorper purus aliquam eget. Aliquam consectetur dolor at erat aliquet, quis suscipit nibh molestie. Proin pharetra hendrerit gravida. Suspendisse potenti. Vestibulum a elementum odio. ', 
			   @EffectiveDate = @TwoHours, 
			   @CallingUserId = 2, 
			   @IsPubliclyVisible = 1
EXEC o.PostAdd @UserId = 2, 
			   @Subject = 'Okay, let''s do this.', 
			   @Body = '

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam nulla nisl, luctus id nunc eget, cursus congue augue. Praesent lectus metus, pulvinar quis ante venenatis, maximus ullamcorper erat. Proin non tincidunt augue. Mauris dignissim tellus sed magna convallis, nec blandit nisl feugiat. Etiam fermentum turpis ac lacus sollicitudin egestas luctus at enim. Mauris ligula nulla, venenatis ut lobortis sed, vestibulum non nibh. Aenean vestibulum placerat aliquam. Praesent ac consequat ipsum, eu facilisis nisi. Aenean dictum justo non tortor auctor ultrices. Sed non ipsum quis lacus tempus sodales ac vel velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean porta eros ligula, et eleifend nisi bibendum vitae. Cras vel elit vitae dui tincidunt aliquet ut ac leo. Interdum et malesuada fames ac ante ipsum primis in faucibus. Praesent ipsum tellus, luctus id lorem dignissim, venenatis tempor leo.

Donec sem odio, sollicitudin nec mi at, vehicula venenatis risus. Aenean condimentum nibh tortor, at imperdiet risus elementum ac. Curabitur pulvinar nulla a suscipit ornare. Mauris at cursus nisl. Aenean porttitor at nunc ac tempus. Etiam interdum non lectus et tincidunt. Mauris quis imperdiet lorem.

In et venenatis neque, in lobortis orci. Mauris eu leo tortor. Vivamus vitae tincidunt dolor. Duis posuere porttitor mi sed pulvinar. Donec non ultricies velit, a convallis turpis. Vestibulum feugiat purus vitae hendrerit placerat. Praesent aliquam sem nec eros rutrum, vitae ultricies eros lacinia. Sed id cursus nisi. Nulla blandit nec lorem in eleifend. Aenean hendrerit euismod leo non volutpat. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Sed posuere eleifend ipsum nec varius. Cras a ornare elit. Aenean eu tempor erat. Interdum et malesuada fames ac ante ipsum primis in faucibus. Etiam commodo imperdiet ligula. ', 
			   @EffectiveDate = @FourHours, 
			   @CallingUserId = 2, 
			   @IsPubliclyVisible = 1
--- TODO: how do I know where to fit the link in the LinkText field?
--INSERT INTO o.Links
--(FK_CreatedByUserId, Title, Body, LinkText, Url, IsVisible)
--VALUES
--(2, 'Every kid needs a champion', 'Rita Pierson, a teacher for 40 years, once heard a colleague say, "They don''t pay me to like the kids." Her response: 
--"Kids don''t learn from people they don’t like.’” A rousing call to educators to believe in their students and actually 
--connect with them on a real, human, personal level.', 'Click here to watch', 'https://www.ted.com/talks/rita_pierson_every_kid_needs_a_champion?language=en', 1);
---
EXEC o.CommentAdd @UserId = 1, @CallingUserId = 1, @PostId = 1, @ProvidedName = 'Orange', @Body = 'Wooo! This is a comment!', @EditKey = 'Fg9$56Jc'
EXEC o.CommentAdd @UserId = 1, @CallingUserId = 1, @PostId = 1, @ProvidedName = 'Orange', @Body = 'No, fuck you!', @EditKey = 'L<.#2@kA'
EXEC o.CommentAdd @UserId = 1, @CallingUserId = 1, @PostId = 2, @ProvidedName = 'Orange', @Body = 'Very pedantic. Great post.', @EditKey = 'xPI3*(nb'
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
---
INSERT INTO o.Tags
(Name)
VALUES
('Teaching'),
('Teachers'),
('Early Childhood Education'),
('Developmentally Appropriate'),
('Mike Is Amazing');
---
INSERT INTO o.TagMap
(FK_PostId, FK_TagId)
VALUES
(1, 1),
(1, 3),
(1, 4),
(2, 3),
(2, 4);