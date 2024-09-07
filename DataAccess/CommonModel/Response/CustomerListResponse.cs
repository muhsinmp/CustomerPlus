using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CommonModel.Response
{
    public class CustomerListResponse
    {
        public bool isSuccess { get; set; }
        public List<CustomerModel> CustomerList { get; set; }
        public string message { get; set; }
    }
}
