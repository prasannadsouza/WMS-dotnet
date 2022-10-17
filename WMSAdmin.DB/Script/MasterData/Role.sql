IF Not EXISTS (SELECT * FROM [dbo].[Role] WHERE [Code] = 'ADMIN')
BEGIN
INSERT [dbo].[Role] ([Code],[RoleName],[Description]) VALUES (N'ADMIN', N'JLM Admin',  N'User that manages JLM Databases, Users and Applications')
END

IF Not EXISTS (SELECT * FROM [dbo].[Role] WHERE [Code] = 'USER')
BEGIN
INSERT [dbo].[Role] ([Code],[RoleName],[Description]) VALUES (N'USER', N'User', N'User of the appliaction')
END