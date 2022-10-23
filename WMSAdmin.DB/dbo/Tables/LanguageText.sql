CREATE TABLE [dbo].[LanguageText]
(
	[Id] BIGINT IDENTITY NOT NULL,
	[LanguageGroupId] BIGINT NOT NULL,
	[LanguageCultureId] BIGINT NOT NULL,
	[Code] NVARCHAR(250) NOT NULL,
	[Value] NVARCHAR(max) NOT NULL,
	[Description] NVARCHAR(250) NULL,
	[TimeStamp] DATETIME NOT NULL, 
CONSTRAINT [PK_LanguageText] PRIMARY KEY CLUSTERED( [Id] ASC
 ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [FK_LanguageText_LanguageGroup] FOREIGN KEY ([LanguageGroupId]) REFERENCES [LanguageGroup]([Id]), 
 CONSTRAINT [FK_LanguageText_LanguageCulture] FOREIGN KEY ([LanguageCultureId]) REFERENCES [LanguageCulture]([Id]), 
) 
