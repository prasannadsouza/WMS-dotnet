using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Constants
{
    public class ErrorCode
    {
        public const int InvalidCredentials = 200001;
        public const int UnableToValidateToken = 200002;
        public const int UnAuthorized = 200003;
        public const int JWTRenewalRequired = 200004;
        public const int AuthRevalidationRequired = 200005;
    }
}
