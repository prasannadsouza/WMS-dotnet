declare @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'APPLICATION')
declare @configtimestamp datetime = getdate()

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'WEBFOLDER_DOWNLOADPATH')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'WEBFOLDER_DOWNLOADPATH', N'', N'Folder Path for web Downloads hint C:\inetpub\connect.sharespine.com\Sharespine-Transport\Downloads')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PATH_TEMPLATEFILES')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'PATH_TEMPLATEFILES', N'', N'Folder Path to Template Files hint \\192.168.32.xxxx\connect.sharespine.com\Sharespine-Transport\LocalFiles\Templates')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'LOCALFILES_BASEPATH')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'LOCALFILES_BASEPATH', N'', N'Folder Path to Download Files hint \\192.168.32.xxxx\connect.sharespine.com\Sharespine-Transport\LocalFiles')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'LOG_DATABASEQUERIES')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'LOG_DATABASEQUERIES', N'0', N'1 will log database queries to the logger')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'UI_LOCALE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'UI_LOCALE', N'sv-SE', N'The display and input locale of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CONFIG_TIMESTAMP')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'CONFIG_TIMESTAMP', convert(varchar, @configtimestamp ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'APPCODE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'APPCODE', N'WMSADMIN', N'The appcode of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BASEURL')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'BASEURL', N'https://localhost:7103', N'The base url of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BASEURL_INTERNAL')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'BASEURL_INTERNAL', N'https://localhost:7103', N'The internal base url of the application')
END

-- PAGINATION 
SELECT @AppConfigGroupId = (select Id from [dbo].[AppConfigGroup] where Code = 'PAGINATION')
IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'RECORDSPERPAGE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'RECORDSPERPAGE', N'15', N'The total number of records to get at a time')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'MAXIMUM_RECORDSPERPAGE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'MAXIMUM_RECORDSPERPAGE', N'250', N'The max number of records to get at a time')
END
IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'TOTALPAGESTOJUMP')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'TOTALPAGESTOJUMP', N'3', N'The total pages to jump at a time on forward and backward in paging')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'MAXIMUM_RECORDSALLOWEDPERPAGE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'MAXIMUM_RECORDSALLOWEDPERPAGE', N'3', N'The maximum records alloweed per pages (for internal processing)')
END


PRINT 'Creating Application Setting for DEBUG_TEST'
SET @AppConfigGroupId = (select Id from [dbo].[AppConfigGroup] where Code = 'DEBUGTEST')

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'IS_TESTMODE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'IS_TESTMODE', N'1', N'if 1 the application is test mode')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'TEST_USERNAME')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'USERNAME', N'408402', N'The test user id to use with test mode')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'TEST_CUSTOMERNUMBER')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'CUSTOMERNUMBER', N'408402', N'The test customer number id to use with test mode')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'TEST_IMPERSONATING_USERNAME')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'IMPERSONATING_USERNAME', N'408402', N'The test impersonator, leave blank if no impersonation is required')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'DEV_AUTO_LOGIN')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'DEV_AUTO_LOGIN', N'1', N'1 if want to Auto Login to the application')
END


PRINT 'Creating Application Setting for EMAIL'
SET @AppConfigGroupId = (select Id from [dbo].[AppConfigGroup] where Code = 'EMAIL')

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'FROM_EMAIL_ADDRESS')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'FROM_EMAIL_ADDRESS', N'noreply@wmsadmin.com', N'The from email address to be sent')
END

update [dbo].[AppConfig] set [Value] = 'Sharespine Transport' WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'FROM_EMAIL_DISPLAYNAME'
IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'FROM_EMAIL_DISPLAYNAME')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'FROM_EMAIL_DISPLAYNAME', N'WMS Admin', N'The display name for the from email address')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CONTACT_EMAIL_ADDRESS')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'CONTACT_EMAIL_ADDRESS', N'support@wmsadmin.com', N'The email address for the customer to contact ')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CONTACT_EMAIL_DISPLAYNAME')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'CONTACT_EMAIL_DISPLAYNAME', N'WMS Support', N'The display name for the from email address')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'SERVER')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'SERVER', N'smtp.wmsadmin.com', N'The smtp server for email')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PORT')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'PORT', N'1234', N'The smtp port for email')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'USERNAME')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'USERNAME', N'wmsadmin@wmsadmin.com', N'The email user name for authentication')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'USERNAME_EMAILADDRESS')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'USERNAME_EMAILADDRESS', N'wmsadmin@wmsadmin.com', N'The email user name for authentication for trusting')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'EMAIL_DOMAIN_TO_TRUST')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'EMAIL_DOMAIN_TO_TRUST', N'wmsadmin.com', N'The email domain to be trusted')
END


IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PASSWORD')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'PASSWORD', N'abcxxxx', N'The email password for authentication')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BCC_EMAILS_FOR_IMPORTANT_INFORMATION')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'BCC_EMAILS_FOR_IMPORTANT_INFORMATION', N'invoice@wmsadmin.com', N'The internal people who need to see important emails seperated by commas')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BCC_EMAILS_FOR_SUPPORT_INFORMATION')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'BCC_EMAILS_FOR_SUPPORT_INFORMATION', N'support1@wmsadmin.com', N'The support people who need to see error or other information  seperated by commas')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'ENABLE_SSL')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'ENABLE_SSL', N'1', N'1 for True')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'EMAIL_LIST_SEPERATOR')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'EMAIL_LIST_SEPERATOR', N',', N'The seperator for emails if multiple emails are specified in email settings of cc and bcc')
END

SELECT @AppConfigGroupId = (select Id from [dbo].[AppConfigGroup] where Code = 'CONFIG_TIMESTAMP')

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'APPLICATION')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'APPLICATION', convert(varchar, @configtimestamp ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'EMAIL')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'EMAIL', convert(varchar, @configtimestamp ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PAGINATION')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'PAGINATION', convert(varchar, @configtimestamp ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'DEBUGTEST')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'DEBUGTEST', convert(varchar, @configtimestamp ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END