using DataAccess.CommonModel.Response;
using DataAccess.DBContext;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Customer
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly AppDBContext _context;

        public CustomerRepo(AppDBContext context) {
            _context = context;
        }

        public async Task<CommonResponse> AddCustomer(CustomerModel model)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                await _context.Customer.AddAsync(model);
                await _context.SaveChangesAsync();

                response.isSuccess = true;
                response.Message = "Customer created. Customer ID : " + model.Id;
            }
            catch (Exception e)
            {
                response.isSuccess =false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<CommonResponse> EditCustomer(CustomerModel model)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (model == null)
                {
                    response.isSuccess = false;
                    response.Message = "Please choose a customer";
                    return response;
                }

                
                if (!await _context.Customer.AnyAsync(x => x.Id == model.Id))
                {
                    response.isSuccess = false;
                    response.Message = "Selected customer ID not found";
                    return response;
                }

                _context.Update(model);
                await _context.SaveChangesAsync();
                response.isSuccess = true;
                response.Message = "Customer updated. Customer ID : " + model.Id;
            
            }
            catch (Exception e)
            {
                response.isSuccess = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<CommonResponse> DeleteCustomer(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (id == 0)
                {
                    response.isSuccess = false;
                    response.Message = "Please choose a customer";
                    return response;
                }


                if (!await _context.Customer.AnyAsync(x => x.Id == id))
                {
                    response.isSuccess = false;
                    response.Message = "Selected customer ID not found";
                    return response;
                }
                CustomerModel model = await _context.Customer.Where(x => x.Id == id).FirstOrDefaultAsync();
                _context.Customer.Remove(model);
                await _context.SaveChangesAsync();

                response.isSuccess = true;
                response.Message = "Customer deleted. Customer ID : " + model.Id;
            }
            catch (Exception e)
            {
                response.isSuccess = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<CustomerListResponse> GetCustomerList()
        {
            CustomerListResponse response = new CustomerListResponse();
            try
            {
               if (!await _context.Customer.AnyAsync())
                {
                    response.isSuccess = true;
                    response.message = "No customers to list";
                    return response;
                }

               response.CustomerList = await _context.Customer.ToListAsync();
                response.isSuccess = true;
                response.message = "success";

                return response;

            }
            catch (Exception e)
            {
                response.isSuccess =false;
                response.message = e.Message;
            }
            return response;
        }

        public async Task<CustomerListResponse> GetCustomerById(int id)
        {
            CustomerListResponse response = new CustomerListResponse();
            try
            {
                if (!await _context.Customer.AnyAsync())
                {
                    response.isSuccess = true;
                    response.message = "No customers to list";
                    return response;
                }

                response.CustomerList = await _context.Customer.Where(x => x.Id == id).ToListAsync();
                response.isSuccess = true;
                response.message = "success";

                return response;

            }
            catch (Exception e)
            {
                response.isSuccess = false;
                response.message = e.Message;
            }
            return response;
        }
    }
}
