declare @AppUserTypeId BIGINT
declare @AppUserId BIGINT
declare @AppCustomerId BIGINT

IF Not EXISTS (SELECT * FROM [dbo].[AppCustomer] WHERE [CustomerNumber] = 'WMSAdmin')
BEGIN
INSERT INTO [dbo].[AppCustomer] ([CustomerNumber],[CustomerName],[Email],[Phone],[TimeStamp]) 
VALUES ('WMSADMIN','WMS Admin','admin@wmsadmin.com','0734875645', GETDATE())
END

select @AppCustomerId = (select [Id] FROM [dbo].[AppCustomer] WHERE [CustomerNumber] = 'WMSAdmin')

select @AppUserTypeId = (select [Id] FROM [dbo].[AppUserType] WHERE [Code] = 'SYSTEMUSER')
IF Not EXISTS (SELECT * FROM [dbo].[AppUser] WHERE [AuthId] = 'SU')
BEGIN
INSERT INTO [dbo].[AppUser] ([AppCustomerId], [AppUserTypeId], [AuthId], [DisplayName], [TimeStamp]) 
VALUES (@AppCustomerId, @AppUserTypeId, 'SU', 'System', GETDATE())
END

select @AppUserTypeId = (select [Id] FROM [dbo].[AppUserType] WHERE [Code] = 'APPUSER')
IF Not EXISTS (SELECT * FROM [dbo].[AppUser] WHERE [AuthId] = 'LU')
BEGIN
INSERT INTO [dbo].[AppUser] ([AppCustomerId], [AppUserTypeId], [AuthId],[AuthSecret], [DisplayName], [TimeStamp]) 
VALUES (@AppCustomerId, @AppUserTypeId, 'LU', '1234', 'LoginUser', GETDATE())
END

select @AppUserId = (SELECT [Id] FROM [dbo].[AppUser] WHERE [AuthId] = 'LU')
IF Not EXISTS (SELECT * FROM [dbo].[AppCustomerUser] WHERE [AppUserId] = @AppUserId)
BEGIN
INSERT INTO [dbo].[AppCustomerUser] ([AppCustomerId],[FirstName], [LastName], [Email], [Phone],[AppUserId],[TimeStamp]) 
VALUES (@AppCustomerId, 'Login','User', 'LoginUser@wmsadmin.com','0734875645', @AppUserId,GETDATE())

END
