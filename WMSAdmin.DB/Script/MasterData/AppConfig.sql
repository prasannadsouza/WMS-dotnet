declare @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'APPLICATION')
IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'UI_LOCALE')
BEGIN
INSERT [dbo].[AppConfig] ([AppConfigGroupId],[Code],[Value], [Description]) 
VALUES (@AppConfigGroupId, N'UI_LOCALE', N'sv-SE', N'The display and input locale of the application')
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
