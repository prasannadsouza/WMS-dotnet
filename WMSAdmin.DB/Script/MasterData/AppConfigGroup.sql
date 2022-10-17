IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'APPLICATION')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Code],[GroupName], [Description]) VALUES (N'APPLICATION', N'Application', N'Group for Application')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'DEBUGTEST')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Code],[GroupName], [Description]) VALUES (N'DEBUGTEST', N'DEBUGTEST', N'Group for Debugging in Test Mode')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'EMAIL')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Code],[GroupName], [Description]) VALUES (N'EMAIL', N'EMAIL', N'Group for Email')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'PAGINATION')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Code],[GroupName], [Description]) VALUES (N'PAGINATION', N'PAGINATION', N'Group for Pagination')
END