﻿CREATE TABLE [dbo].[AppUserType]
(
	[Id] BIGINT IDENTITY NOT NULL,
	[Code] NVARCHAR(250) NOT NULL,
	[AppUserTypeName] NVARCHAR(250) NOT NULL,
	[Description] NVARCHAR(250) NOT NULL,
	[TimeStamp] DATETIME2 NOT NULL, 
CONSTRAINT [PK_AppAuthType] PRIMARY KEY CLUSTERED( [Id] ASC
 ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) 