using DataAccess.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Customer;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.Admin)]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepo _customerRepo;

        public CustomerController(ICustomerRepo customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet]
        [Route("getCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerRepo.GetCustomerList();

            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("getCustomerById")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var result = await _customerRepo.GetCustomerById(id);

            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("createCustomer")]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerModel model)
        {
            if (string.IsNullOrEmpty(model.FirstName))
            {
                return BadRequest("Please provide first name");
            }

            if (string.IsNullOrEmpty(model.LastName))
            {
                return BadRequest("Please provide last name");
            }

            var result = await _customerRepo.AddCustomer(model);
            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("editCustomer")]
        public async Task<IActionResult> EditCustomer([FromBody] CustomerModel model)
        {
            

            var result = await _customerRepo.EditCustomer(model);
            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("deleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {


            var result = await _customerRepo.DeleteCustomer(id);
            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
