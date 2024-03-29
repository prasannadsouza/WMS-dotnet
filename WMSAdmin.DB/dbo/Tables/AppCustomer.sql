﻿CREATE TABLE [dbo].[AppCustomer]
(
	[Id] BIGINT IDENTITY NOT NULL,
	[CustomerNumber] NVARCHAR(250) NOT NULL,
	[CustomerName] NVARCHAR(250) NOT NULL,
	[OrganizationNumber] NVARCHAR(250) NOT NULL,
	[Email] NVARCHAR(250) NOT NULL,
	[LocaleCode] NVARCHAR(50) NULL,
	[Phone] NVARCHAR(250) NOT NULL,
	[TimeStamp] DATETIME2 NOT NULL, 
CONSTRAINT [PK_AppCustomer] PRIMARY KEY CLUSTERED( [Id] ASC
 ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
) 

