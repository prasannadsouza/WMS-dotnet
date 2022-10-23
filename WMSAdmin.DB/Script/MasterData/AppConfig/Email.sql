declare @AppConfigGroupId BIGINT = (select Id from [dbo].[AppConfigGroup] where Code = 'EMAIL')
PRINT 'Creating data for AppConfig EMAIL'

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'FROM_EMAIL_ADDRESS')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'FROM_EMAIL_ADDRESS', N'noreply@wmsadmin.com', N'The from email address to be sent')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'FROM_EMAIL_DISPLAYNAME')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'FROM_EMAIL_DISPLAYNAME', N'WMS Admin', N'The display name for the from email address')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CONTACT_EMAIL_ADDRESS')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'CONTACT_EMAIL_ADDRESS', N'support@wmsadmin.com', N'The email address for the customer to contact ')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'CONTACT_EMAIL_DISPLAYNAME')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'CONTACT_EMAIL_DISPLAYNAME', N'WMS Support', N'The display name for the from email address')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'SERVER')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'SERVER', N'smtp.wmsadmin.com', N'The smtp server for email')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PORT')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'PORT', N'1234', N'The smtp port for email')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'USERNAME')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'USERNAME', N'wmsadmin@wmsadmin.com', N'The email user name for authentication')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'USERNAME_EMAILADDRESS')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'USERNAME_EMAILADDRESS', N'wmsadmin@wmsadmin.com', N'The email user name for authentication for trusting')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'EMAIL_DOMAIN_TO_TRUST')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'EMAIL_DOMAIN_TO_TRUST', N'wmsadmin.com', N'The email domain to be trusted')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'PASSWORD')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'PASSWORD', N'abcxxxx', N'The email password for authentication')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BCC_EMAILS_FOR_IMPORTANT_INFORMATION')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'BCC_EMAILS_FOR_IMPORTANT_INFORMATION', N'invoice@wmsadmin.com', N'The internal people who need to see important emails seperated by commas')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'BCC_EMAILS_FOR_SUPPORT_INFORMATION')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'BCC_EMAILS_FOR_SUPPORT_INFORMATION', N'support1@wmsadmin.com', N'The support people who need to see error or other information  seperated by commas')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'ENABLE_SSL')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'ENABLE_SSL', N'1', N'1 for True')
END

IF Not EXISTS (SELECT * FROM [dbo].[AppConfig] WHERE [AppConfigGroupId] = @AppConfigGroupId and [Code] = 'EMAIL_LIST_SEPERATOR')
BEGIN
INSERT [dbo].[AppConfig] ([TimeStamp], [AppConfigGroupId],[Code],[Value], [Description])  
VALUES (GETDATE(), @AppConfigGroupId,  N'EMAIL_LIST_SEPERATOR', N',', N'The seperator for emails if multiple emails are specified in email settings of cc and bcc')
END
