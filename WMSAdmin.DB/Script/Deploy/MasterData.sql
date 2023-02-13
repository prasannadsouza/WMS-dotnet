use  [WMSAdmin]
GO

PRINT 'Executing AppConfig'
:r ../MasterData/AppConfig/Application.sql
GO
:r ../MasterData/AppConfig/DebugTest.sql
GO
:r ../MasterData/AppConfig/Email.sql
GO
:r ../MasterData/AppConfig/Pagination.sql
GO
:r ../MasterData/AppConfig/JwtToken.sql
GO
PRINT 'Executing Role'
:r ../MasterData/Role.sql
GO
PRINT 'AppUserType'
:r ../MasterData/AppUserType.sql
GO
PRINT 'AppUser'
:r ../MasterData/AppUser.sql
GO
PRINT 'WMS Application'
:r ../MasterData/WMSApplication.sql
GO
PRINT 'Language Culture'
:r ../MasterData/Language/LanguageCulture.sql
GO
PRINT 'Language WMS Admin General'
:r ../MasterData/Language/WMSAdmin/General.sql
GO
PRINT 'Language WMS Admin Login'
:r ../MasterData/Language/WMSAdmin/Login.sql
GO