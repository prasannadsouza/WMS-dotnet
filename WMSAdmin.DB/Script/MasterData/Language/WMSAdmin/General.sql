declare @WMSApplicationId BIGINT = (select Id from [dbo].[WMSApplication] where Code = 'WMSAdmin')
declare @LanguageCultureSwedish BIGINT = (select Id from [dbo].[LanguageCulture] where Code = 'sv-SE')
declare @LanguageCultureEnglish BIGINT = (select Id from [dbo].[LanguageCulture] where Code = 'en-SE')
declare @LangugaeGroupCode NVARCHAR(50) = N'GENERAL'
declare @Timestamp DATETIME = GetDate()
IF Not EXISTS (SELECT * FROM [dbo].[LanguageGroup] WHERE WMSApplicationId = @WMSApplicationId and [Code] = @LangugaeGroupCode)
INSERT [dbo].[LanguageGroup] ([Timestamp], [WMSApplicationId],[Code],[Name]) VALUES (@Timestamp,@WMSApplicationId, @LangugaeGroupCode, N'Group for general text')

declare @LanguageGroupId BIGINT = (SELECT Id FROM [dbo].[LanguageGroup] WHERE WMSApplicationId = @WMSApplicationId and [Code] = @LangugaeGroupCode)

delete from [LanguageText] where [LanguageGroupId] = @LanguageGroupId
insert into [LanguageText] ([Timestamp],[LanguageCultureId],[LanguageGroupId],[Code],[Value])
select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'All','All' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'All','Alla' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Yes','Yes' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Yes','Ja' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'No','No' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'No','Nej' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Ok','Ok' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Ok','Ok' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'To','To' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'To','Till' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Home','Home' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Home','Hem' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Settings','Settings' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Settings','Installingar' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Cancel','Cancel' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Cancel','Avbryt' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Email','Email' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Email','E-post' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'ConfirmTitle','Please confirm' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'ConfirmTitle','Vänligen bekräfta' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'ConfirmMessage','Are you sure?' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'ConfirmMessage','Är du säker?' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Message','Message' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Message','Meddelande' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Error','Error' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Error','Fel' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Close','Close' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Close','Stänga' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'FetchData','Fetch Data' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'FetchData','Hämta data' union 

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Language','Language' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Language','Språk'

