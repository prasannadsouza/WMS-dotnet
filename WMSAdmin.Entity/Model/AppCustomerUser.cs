using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Model
{
    public class AppCustomerUser
    {
        public long? Id { get; set; }
        public long? AppCustomerId { get; set; }
        public string DisplayName { get; set; }
        public string LocaleCode { get; set; }
        public DateTime? LoginTime { get; set; }
    }
}
