declare @WMSApplicationId BIGINT = (select Id from [dbo].[WMSApplication] where Code = 'WMSAdmin')
declare @LanguageCultureSwedish BIGINT = (select Id from [dbo].[LanguageCulture] where Code = 'sv-SE')
declare @LanguageCultureEnglish BIGINT = (select Id from [dbo].[LanguageCulture] where Code = 'en-SE')
declare @LangugaeGroupCode NVARCHAR(50) = N'GENERAL'
declare @TimeStamp DATETIME2 = GetDate()
IF Not EXISTS (SELECT * FROM [dbo].[LanguageGroup] WHERE WMSApplicationId = @WMSApplicationId and [Code] = @LangugaeGroupCode)
INSERT [dbo].[LanguageGroup] ([TimeStamp], [WMSApplicationId],[Code],[Name]) VALUES (@TimeStamp,@WMSApplicationId, @LangugaeGroupCode, N'Group for general text')

declare @LanguageGroupId BIGINT = (SELECT Id FROM [dbo].[LanguageGroup] WHERE WMSApplicationId = @WMSApplicationId and [Code] = @LangugaeGroupCode)

delete from [LanguageText] where [LanguageGroupId] = @LanguageGroupId
insert into [LanguageText] ([Timestamp],[LanguageCultureId],[LanguageGroupId],[Code],[Value])
select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'All','All' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'All','Alla' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Yes','Yes' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Yes','Ja' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'No','No' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'No','Nej' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Ok','Ok' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Ok','Ok' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'To','To' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'To','Till' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Home','Home' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Home','Hem' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Settings','Settings' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Settings','Installingar' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Cancel','Cancel' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Cancel','Avbryt' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Email','Email' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Email','E-post' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'ConfirmTitle','Please confirm' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'ConfirmTitle','Vänligen bekräfta' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'ConfirmMessage','Are you sure?' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'ConfirmMessage','Är du säker?' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Message','Message' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Message','Meddelande' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Error','Error' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Error','Fel' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Close','Close' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Close','Stänga' union

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'FetchData','Fetch Data' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'FetchData','Hämta data' union 

select @TimeStamp,@LanguageCultureEnglish,@LanguageGroupId,'Language','Language' union
select @TimeStamp,@LanguageCultureSwedish,@LanguageGroupId,'Language','Språk'

