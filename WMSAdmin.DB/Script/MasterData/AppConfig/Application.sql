PRINT 'Creating data for AppConfig APPLICATION'

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'APPLICATION')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'APPLICATION', N'Application', N'Group for Application')
END

declare @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'APPLICATION')

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'TITLE')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'TITLE', N'WMS Admin', N'Title of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CURRENT_VERSION')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'CURRENT_VERSION', N'0.1', N'Title of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'WEBFOLDER_DOWNLOADPATH')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'WEBFOLDER_DOWNLOADPATH', N'', N'Folder Path for web Downloads hint C:\inetpub\connect.sharespine.com\Sharespine-Transport\Downloads')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PATH_TEMPLATEFILES')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'PATH_TEMPLATEFILES', N'', N'Folder Path to Template Files hint \\192.168.32.xxxx\connect.sharespine.com\Sharespine-Transport\LocalFiles\Templates')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'LOCALFILES_BASEPATH')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'LOCALFILES_BASEPATH', N'', N'Folder Path to Download Files hint \\192.168.32.xxxx\connect.sharespine.com\Sharespine-Transport\LocalFiles')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'LOG_DATABASEQUERIES')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'LOG_DATABASEQUERIES', N'0', N'1 will log database queries to the logger')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'LOCALE')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'LOCALE', N'sv-SE', N'The locale of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'UI_LOCALE')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'UI_LOCALE', N'sv-SE', N'The display and input locale of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'APPCODE')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'APPCODE', N'WMSADMIN', N'The appcode of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BASEURL')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'BASEURL', N'https://localhost:7103', N'The base url of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BASEURL_INTERNAL')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'BASEURL_INTERNAL', N'https://localhost:7103', N'The internal base url of the application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CACHEEXPIRY_INMINUTES')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'CACHEEXPIRY_INMINUTES', N'60', N'The expiry of items in the cache')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'SYSTEMUSER_CODE')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'SYSTEMUSER_CODE', N'SU', N'The system user code')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'JWTKEY')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'JWTKEY', N'RüZ%Ü¡yóöIc¸òñtÕqa7ð¢ÖZ#ÍNÂ9È|â¹ºïs!¥EìÜËÂh¡~üÕ"7O×EJ²CMüëªÖ', N'The Jwt key')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'JWTISSUER')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'JWTISSUER', N'WMS Admin', N'The JWT Issuer')
END