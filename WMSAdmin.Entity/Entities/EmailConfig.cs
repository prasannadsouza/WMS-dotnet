using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class EmailConfig
    {
        public string FromEmailAdddress { get; set; }
        public string FromEmailDisplayName { get; set; }
        public string ContactEmailAdddress { get; set; }
        public string ContactEmailDisplayName { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string UserNameEmailAddress { get; set; }
        public string EmailDomainToTrust { get; set; }
        public string Password { get; set; }
        public string BccEmailsImportantInformation { get; set; }
        public string BccEmailsSupportInformation { get; set; }
        public bool EnableSSL { get; set; }
        public string EmailListSeperator { get; set; }
    }
}
