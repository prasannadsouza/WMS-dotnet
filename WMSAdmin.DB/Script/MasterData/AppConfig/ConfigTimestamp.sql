DECLARE @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'CONFIG_TIMESTAMP')
PRINT 'Creating data for AppConfig CONFIG_TIMESTAMP'

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'APPLICATION')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId, N'APPLICATION', convert(varchar, GETDATE() ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'EMAIL')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId, N'EMAIL', convert(varchar, GETDATE() ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PAGINATION')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId, N'PAGINATION', convert(varchar, GETDATE() ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'DEBUGTEST')
BEGIN
INSERT [dbo].[AppConfig] ([Timestamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId, N'DEBUGTEST', convert(varchar, GETDATE() ,121) , N'The date time in invariant format upto seconds if config timestamp is changed')
END