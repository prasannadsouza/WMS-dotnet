PRINT 'Creating Data for AppConfigGroup'

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'APPLICATION')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'APPLICATION', N'Application', N'Group for Application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'DEBUGTEST')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'DEBUGTEST', N'DEBUGTEST', N'Group for Debugging in Test Mode')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'EMAIL')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'EMAIL', N'EMAIL', N'Group for Email')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'PAGINATION')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'PAGINATION', N'PAGINATION', N'Group for Pagination')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'CONFIG_TIMESTAMP')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'CONFIG_TIMESTAMP', N'CONFIG_TIMESTAMP', N'Group for Config Timestamps')
END