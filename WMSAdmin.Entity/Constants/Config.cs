using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Constants
{
    public static class Config
    {
        public const string GROUP_APPLICATION = "APPLICATION";
        public const string APPLICATION_UI_LOCALE = "UI_LOCALE";
        public const string APPLICATION_PAGINATION_RECORDSPERPAGE = "PAGINATION_RECORDSPERPAGE";
        public const string APPLICATION_PAGINATION_MAXIMUM_RECORDSPERPAGE = "PAGINATION_MAXIMUM_RECORDSPERPAGE";
        public const string APPLICATION_PAGINATION_TOTALPAGESTOJUMP = "PAGINATION_TOTALPAGESTOJUMP";
        public const string APPLICATION_TITLE = "TITLE";
        public const string APPLICATION_CURRENT_VERSION = "CURRENT_VERSION";
        public const string APPLICATION_ADMINURL = "ADMINURL";
        public const string APPLICATION_ADMINURL_INTERNAL = "ADMINURL_INTERNAL";
       
        public const string APPLICATION_LOG_DATABASEQUERIES = "LOG_DATABASEQUERIES";
        public const string APPLICATION_LOCALE = "LOCALE";
        public const string APPLICATION_APPCODE = "APPCODE";
        public const string APPLICATION_WEBFOLDER_DOWNLOADPATH = "WEBFOLDER_DOWNLOADPATH";
        public const string APPLICATION_PATH_TEMPLATEFILES = "PATH_TEMPLATEFILES";
        public const string APPLICATION_APP_APIKEY = "APP_APIKEY";
        public const string APPLICATION_MAXSECONDSTO_RETAINDOWNLOADFILES = "MAXSECONDSTO_RETAINDOWNLOADFILES";
        public const string APPLICATION_LOCALFILES_BASEPATH = "LOCALFILES_BASEPATH";
        public const string APPLICATION_SESSIONEXPIRESINMINUTES = "SESSIONEXPIRESINMINUTES";

        public const string GROUP_EMAIL = "EMAIL";
        public const string EMAIL_FROM_EMAIL_ADDRESS = "FROM_EMAIL_ADDRESS";
        public const string EMAIL_CONTACT_EMAIL_ADDRESS = "CONTACT_EMAIL_ADDRESS";
        public const string EMAIL_SERVER = "SERVER";
        public const string EMAIL_PORT = "PORT";
        public const string EMAIL_USERNAME = "USERNAME";
        public const string EMAIL_USERNAME_EMAILADDRESS = "USERNAME_EMAILADDRESS";
        public const string EMAIL_PASSWORD = "PASSWORD";
        public const string EMAIL_BCC_EMAILS_FOR_IMPORTANT_INFORMATION = "BCC_EMAILS_FOR_IMPORTANT_INFORMATION";
        public const string EMAIL_BCC_EMAILS_FOR_SUPPORT_INFORMATION = "BCC_EMAILS_FOR_SUPPORT_INFORMATION";
        public const string EMAIL_ENABLE_SSL = "ENABLE_SSL";
        public const string EMAIL_LIST_SEPERATOR = "EMAIL_LIST_SEPERATOR";
        public const string EMAIL_FROM_EMAIL_DISPLAYNAME = "FROM_EMAIL_DISPLAYNAME";
        public const string EMAIL_CONTACT_EMAIL_DISPLAYNAME = "CONTACT_EMAIL_DISPLAYNAME";
        public const string EMAIL_EMAIL_DOMAIN_TO_TRUST = "EMAIL_DOMAIN_TO_TRUST";

        public const string GROUP_DEBUGTEST = "DEBUGTEST";
        public const string DEBUGTEST_IISEXPRESS_TESTMODE = "IISEXPRESS_TESTMODE";
        public const string DEBUGTEST_IS_TESTMODE = "IS_TESTMODE";
        public const string DEBUGTEST_TEST_USERNAME = "TEST_USERNAME";
        public const string DEBUGTEST_TEST_CUSTOMERNUMBER = "TEST_CUSTOMERNUMBER";
        public const string DEBUGTEST_TEST_IMPERSONATING_USERNAME = "TEST_IMPERSONATING_USERNAME";
        public const string DEBUGTEST_DEV_AUTO_LOGIN = "DEV_AUTO_LOGIN";

        public const string GROUP_CONFIGTIMESTAMP = "GROUP_CONFIGTIMESTAMP";
        public const string CONFIGTIMESTAMP_CONFIGSETTING = "CONFIGSETTING";
        public const string CONFIGTIMESTAMP_LANGUAGETEXT = "LANGUAGETEXT";
        public const string CONFIGTIMESTAMP_APPLICATION = "APPLICATION";

        public const string TRUE_VALUE = "1";
        public const string FALSE_VALUE = "0";
    }
}
