using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CommonModel.Response
{
    public class CommonResponse
    {
        public bool isSuccess { get; set; } = false;
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
