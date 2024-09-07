using DataAccess.CommonModel.Response;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Customer
{
    public interface ICustomerRepo
    {
        Task<CommonResponse> AddCustomer(CustomerModel model);
        Task<CommonResponse> EditCustomer(CustomerModel model);
        Task<CommonResponse> DeleteCustomer(int id);
        Task<CustomerListResponse> GetCustomerList();
        Task<CustomerListResponse> GetCustomerById(int id);
    }
}
