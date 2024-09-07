using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CommonModel.Response
{
    public class LoginResponse
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string expiration { get; set; }
    }
}
