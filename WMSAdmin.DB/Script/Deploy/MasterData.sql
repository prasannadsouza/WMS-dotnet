use  [WMSAdmin]

PRINT 'Executing AppConfigGroup'
:r ../MasterData/AppConfigGroup.sql
GO
PRINT 'Executing AppConfig'
:r ../MasterData/AppConfig/Application.sql
GO
:r ../MasterData/AppConfig/ConfigTimestamp.sql
GO
:r ../MasterData/AppConfig/DebugTest.sql
GO
:r ../MasterData/AppConfig/Email.sql
GO
:r ../MasterData/AppConfig/Pagination.sql
GO

PRINT 'Executing Role'
:r ../MasterData/Role.sql
GO
PRINT 'WMS Application'
:r ../MasterData/WMSApplication.sql
GO