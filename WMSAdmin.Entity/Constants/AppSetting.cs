using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Constants
{
    public static class AppSetting
    {
        public const string ConnectionStrings_BaseConnection = "BaseConnection";
    }

    public static class AppUserType
    {
        public const string APPUSER = nameof(APPUSER);
        public const string APIUSER = nameof(APIUSER);
        public const string SYSTEMUSER = nameof(SYSTEMUSER);
        public const string REMOTEAPPADMIN = nameof(REMOTEAPPADMIN);
    }

    public static class AppAccessType
    {
        public const string WEB = nameof(WEB);
        public const string API = nameof(API);
    }


    public static class WebAppSetting
    {
        public const string XAccessToken = "X-Access-Token";
        public const string XRefreshToken = "X-Refresh-Token";
        public const string ContextItemAppUserProfile = "AppUserProfile";
    }
}
