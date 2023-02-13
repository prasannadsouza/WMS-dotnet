using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository.POCO
{
    public class AppCustomer
    {
        public long? Id { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LocaleCode { get; set; }
        public string OrganizationNumber { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
