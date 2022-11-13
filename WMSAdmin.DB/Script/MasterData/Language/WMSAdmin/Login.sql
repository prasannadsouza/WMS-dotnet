declare @WMSApplicationId BIGINT = (select Id from [dbo].[WMSApplication] where Code = 'WMSAdmin')
declare @LanguageCultureSwedish BIGINT = (select Id from [dbo].[LanguageCulture] where Code = 'sv-SE')
declare @LanguageCultureEnglish BIGINT = (select Id from [dbo].[LanguageCulture] where Code = 'en-SE')
declare @LangugaeGroupCode NVARCHAR(50) = N'LOGIN'
declare @Timestamp DATETIME2 = GetDate()
IF Not EXISTS (SELECT * FROM [dbo].[LanguageGroup] WHERE WMSApplicationId = @WMSApplicationId and [Code] = @LangugaeGroupCode)
INSERT [dbo].[LanguageGroup] ([Timestamp], [WMSApplicationId],[Code],[Name]) VALUES (@Timestamp,@WMSApplicationId, @LangugaeGroupCode, N'Group for Login text')

declare @LanguageGroupId BIGINT = (SELECT Id FROM [dbo].[LanguageGroup] WHERE WMSApplicationId = @WMSApplicationId and [Code] = @LangugaeGroupCode)

delete from [LanguageText] where [LanguageGroupId] = @LanguageGroupId
insert into [LanguageText] ([Timestamp],[LanguageCultureId],[LanguageGroupId],[Code],[Value])
select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Login','Login' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Login','Logga in' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Logout','Logout' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Logout','Logga ut' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Username','Username' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Username','Användarnamn' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'Password','Password' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'Password','Lösenord' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'ForgotPassword','Forgot Password' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'ForgotPassword','Glömt lösenord' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'SendPasswordResetLink','Send Reset Link' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'SendPasswordResetLink','Skicka återställningslänk' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'UsernameCannotBeBlank','Username is required' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'UsernameCannotBeBlank','Användarnamn krävs' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'UsernameOrPasswordIsInvalid','Username or password is invalid' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'UsernameOrPasswordIsInvalid','Användarnamnet eller lösenordet är ogiltigt'  union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'EmailCannotBeBlank','Email is required' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'EmailCannotBeBlank','E-Post krävs'  union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'PasswordCannotBeBlank','Password is required' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'PasswordCannotBeBlank','Lösenord krävs'  union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'ResetEmailLinkSent','Reset Email Link Sent' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'ResetEmailLinkSent','Återställ e-postlänk skickad' union

select @Timestamp,@LanguageCultureEnglish,@LanguageGroupId,'LoginTitle','Login' union
select @Timestamp,@LanguageCultureSwedish,@LanguageGroupId,'LoginTitle','Logga in' 
