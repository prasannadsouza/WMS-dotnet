PRINT 'Creating Data for WMSApplication'

IF Not EXISTS (SELECT * FROM [dbo].[WMSApplication] WHERE [Code] = 'WMSAdmin')
BEGIN
INSERT [dbo].[WMSApplication] ([Timestamp], [Code],[Name], [Description]) VALUES (GETDATE(), N'WMSAdmin', N'WMSAdmin', N'App to manage WMS Applications')
END
