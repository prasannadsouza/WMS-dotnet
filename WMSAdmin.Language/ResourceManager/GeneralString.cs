namespace WMSAdmin.Language.ResourceManager
{
    public class GeneralString : BaseResourceManager
    {
        public GeneralString(Utility.Configuration configuration, System.Globalization.CultureInfo culture) : base(configuration, culture, Entity.Constants.Language.LANGUAGEGROUP_GENERAL)
        {
        }
        public string All => GetResourceString(nameof(All));
        public string Cancel => GetResourceString(nameof(Cancel));
        public string Close => GetResourceString(nameof(Close));
        public string ConfirmMessage => GetResourceString(nameof(ConfirmMessage));
        public string ConfirmTitle => GetResourceString(nameof(ConfirmTitle));
        public string Email => GetResourceString(nameof(Email));
        public string Error => GetResourceString(nameof(Error));
        public string FetchData => GetResourceString(nameof(FetchData));
        public string Home => GetResourceString(nameof(Home));
        public string Language => GetResourceString(nameof(Language));
        public string Message => GetResourceString(nameof(Message));
        public string No => GetResourceString(nameof(No));
        public string Ok => GetResourceString(nameof(Ok));
        public string Settings => GetResourceString(nameof(Settings));
        public string To => GetResourceString(nameof(To));
        public string Yes => GetResourceString(nameof(Yes));

        public Entity.Entities.LanguageStrings.GeneralString GetString()
        {
            return new Entity.Entities.LanguageStrings.GeneralString
            {
                All = All,
                Cancel = Cancel,
                Close = Close,
                ConfirmMessage = ConfirmMessage,
                ConfirmTitle = ConfirmTitle,
                Email = Email,
                Error = Error,
                FetchData = FetchData,
                Home = Home,
                Language = Language,
                Message = Message,
                No = No,
                Ok = Ok,
                Settings = Settings,
                To = To,
                Yes = Yes,
            };
        }
    }
}
