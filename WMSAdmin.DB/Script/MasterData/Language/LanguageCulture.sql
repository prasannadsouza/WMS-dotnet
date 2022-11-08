IF Not EXISTS (SELECT * FROM [dbo].[LanguageCulture] WHERE [Code] = 'sv-SE')
INSERT [dbo].[LanguageCulture] ([Timestamp],[Code],[Name]) VALUES (GETDATE(),N'sv-SE', N'Sverige (Svenska)')

IF Not EXISTS (SELECT * FROM [dbo].[LanguageCulture] WHERE [Code] = 'en-SE')
INSERT [dbo].[LanguageCulture] ([Timestamp],[Code],[Name]) VALUES (GETDATE(),N'en-SE', N'Sweden (English)')