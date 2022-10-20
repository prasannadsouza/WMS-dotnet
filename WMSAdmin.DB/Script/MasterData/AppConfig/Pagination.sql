declare @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'PAGINATION')
PRINT 'Creating data for AppConfig PAGINATION'

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'RECORDSPERPAGE')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'RECORDSPERPAGE', N'15', N'The total number of records to get at a time')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'MAXIMUM_RECORDSPERPAGE')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'MAXIMUM_RECORDSPERPAGE', N'250', N'The max number of records to get at a time')
END
IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'TOTALPAGESTOJUMP')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'TOTALPAGESTOJUMP', N'3', N'The total pages to jump at a time on forward and backward in paging')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'MAXIMUM_RECORDSALLOWEDPERPAGE')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'MAXIMUM_RECORDSALLOWEDPERPAGE', N'3', N'The maximum records alloweed per pages (for internal processing)')
END