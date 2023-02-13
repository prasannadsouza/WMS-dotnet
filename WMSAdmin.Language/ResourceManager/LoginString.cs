namespace WMSAdmin.Language.ResourceManager
{
    public class LoginString : BaseResourceManager
    {
        public LoginString(Utility.Configuration configuration, System.Globalization.CultureInfo culture) : base(configuration, culture, Entity.Constants.Language.LANGUAGEGROUP_LOGIN)
        {
        }
        public string EmailCannotBeBlank => GetResourceString(nameof(EmailCannotBeBlank));
        public string ForgotPassword => GetResourceString(nameof(ForgotPassword));
        public string Login => GetResourceString(nameof(Login));
        public string Logout => GetResourceString(nameof(Logout));
        public string Password => GetResourceString(nameof(Password));
        public string PasswordCannotBeBlank => GetResourceString(nameof(PasswordCannotBeBlank));
        public string ResetEmailLinkSent => GetResourceString(nameof(ResetEmailLinkSent));
        public string SendPasswordResetLink => GetResourceString(nameof(SendPasswordResetLink));
        public string Username => GetResourceString(nameof(Username));
        public string UsernameCannotBeBlank => GetResourceString(nameof(UsernameCannotBeBlank));
        public string UsernameOrPasswordIsInvalid => GetResourceString(nameof(UsernameOrPasswordIsInvalid));
        public string UnableToValidateToken => GetResourceString(nameof(UnableToValidateToken));

        public Entity.Entities.LanguageStrings.LoginString GetString()
        {
            return new Entity.Entities.LanguageStrings.LoginString
            {
                EmailCannotBeBlank = EmailCannotBeBlank,
                ForgotPassword = ForgotPassword,
                Login = Login,
                Logout = Logout,
                Password = Password,
                PasswordCannotBeBlank = PasswordCannotBeBlank,
                ResetEmailLinkSent = ResetEmailLinkSent,
                SendPasswordResetLink = SendPasswordResetLink,
                Username = Username,
                UsernameCannotBeBlank = UsernameCannotBeBlank,
                UsernameOrPasswordIsInvalid = UsernameOrPasswordIsInvalid,
                UnableToValidateToken = UnableToValidateToken,
            };
        }
    }
}
