PRINT 'Creating data for Role'
IF Not EXISTS (SELECT * FROM [dbo].[Role] WHERE [Code] = 'ADMIN')
BEGIN
INSERT [dbo].[Role] ([Timestamp], [Code],[RoleName],[Description]) VALUES (GETDATE(), N'ADMIN', N'JLM Admin',  N'User that manages JLM Databases, Users and Applications')
END

IF Not EXISTS (SELECT * FROM [dbo].[Role] WHERE [Code] = 'USER')
BEGIN
INSERT [dbo].[Role] ([Timestamp], [Code],[RoleName],[Description]) VALUES (GETDATE(), N'USER', N'User', N'User of the appliaction')
END