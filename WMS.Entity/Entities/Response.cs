using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.Entities
{
    public class Response<T>
    {
        public List<Error> Errors { get; set; }
        public bool IgnoreSessionValidity { get; set; }
        public bool IsSessionValid { get; set; }
        public string Message { get; set; }
        public string AdditionalInformation { get; set; }
        public string CorrelationId { get; set; }
        public T Data { get; set; }
        public long TimeStamp { get; set; }
        public DateTime DateTime { get; set; }
        public Pagination Pagination { get; set; }
    }
}
