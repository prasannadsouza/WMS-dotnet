IF Not EXISTS (SELECT * FROM [dbo].[AppLogin] WHERE [LoginId] = 'SU')
BEGIN
INSERT INTO [dbo].[AppLogin] ([LoginId],[DisplayName],[TimeStamp]) VALUES ('SU','System',GETDATE())
END

IF Not EXISTS (SELECT * FROM [dbo].[AppLogin] WHERE [LoginId] = 'NU')
BEGIN
INSERT INTO [dbo].[AppLogin] ([LoginId],[LoginSecret],[DisplayName],[TimeStamp]) VALUES ('NU','1234', 'SetupUser',GETDATE())
END
