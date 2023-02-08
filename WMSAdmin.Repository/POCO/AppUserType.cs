using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Repository.POCO
{
    public class AppUserType
    {
        public long? Id { get; set; }
        public string Code { get; set; }
        public string AppUserTypeName { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
