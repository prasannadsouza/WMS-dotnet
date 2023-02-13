using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMSAdmin.Entity.Model
{
    public class AuthenticateAppUserResponse
    {
        public AppCustomerUser AppCustomerUser { get; set; }
        public Entities.AppCustomer AppCustomer { get; set; }
    }
}
