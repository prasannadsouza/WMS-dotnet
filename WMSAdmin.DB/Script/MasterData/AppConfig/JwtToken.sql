PRINT 'Creating data for AppConfig JWTTOKEN'

IF Not EXISTS (SELECT * FROM [dbo].[AppConfigGroup] WHERE [Code] = 'JWTTOKEN')
BEGIN
INSERT [dbo].[AppConfigGroup] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'JWTTOKEN', N'JWTTOKEN', N'Group for JwtToken')
END

declare @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'JWTTOKEN')


IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'SECURITYKEY')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'SECURITYKEY', N'RüZ%Ü¡yóöIc¸òñtÕqa7ð¢ÖZ#ÍNÂ9È|â¹ºïs!¥EìÜËÂh¡~üÕ"7O×EJ²CMüëªÖ', N'The Jwt security key')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'ISSUER')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'ISSUER', N'WMS Admin', N'The JWT Issuer')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'VALIDITYINMINUTES')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'VALIDITYINMINUTES', N'1', N'The validaity of JWT Token')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'MAXRENEWALS')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'MAXRENEWALS', N'3', N'The max renewal or refresh of JWL TOken s of validaity of JWT Token')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'MAXIDLETIMEINMINUTES')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'MAXIDLETIMEINMINUTES', N'3', N'The max minutes between last access and current access')
END