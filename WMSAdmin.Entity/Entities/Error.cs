using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Entities
{
    public class Error
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string AdditionalCode { get; set; }
        public string AdditionalInformation { get; set; }
        public string SystemError { get; set; }
    }
}
