IF Not EXISTS (SELECT * FROM [dbo].[AppUserType] WHERE [Code] = 'APPUSER')
BEGIN
INSERT INTO [dbo].AppUserType ([Code],[AppUserTypeName],[Description],[TimeStamp]) VALUES ('APPUSER','App User', 'App User',GETDATE())
END

IF Not EXISTS (SELECT * FROM [dbo].[AppUserType] WHERE [Code] = 'APIUSER')
BEGIN
INSERT INTO [dbo].AppUserType ([Code],[AppUserTypeName],[Description],[TimeStamp]) VALUES ('APIUSER','API User', 'API User',GETDATE())
END

IF Not EXISTS (SELECT * FROM [dbo].[AppUserType] WHERE [Code] = 'SYSTEMUSER')
BEGIN
INSERT INTO [dbo].AppUserType ([Code],[AppUserTypeName],[Description],[TimeStamp]) VALUES ('SYSTEMUSER','System User', 'System User',GETDATE())
END

IF Not EXISTS (SELECT * FROM [dbo].[AppUserType] WHERE [Code] = 'REMOTEAPPADMIN')
BEGIN
INSERT INTO [dbo].AppUserType ([Code],[AppUserTypeName],[Description],[TimeStamp]) VALUES ('REMOTEAPPADMIN','Remote Admin', 'Remote Admin',GETDATE())
END