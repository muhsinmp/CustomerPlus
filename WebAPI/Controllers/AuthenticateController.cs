
using DataAccess.CommonModel.Response;
using DataAccess.DBContext;
using DataAccess.Models;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Account;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        
        private readonly IAccountRepo _accountManager;

        public AuthenticateController(IAccountRepo accountManager)
        {
            
            _accountManager = accountManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if(String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Password)) return BadRequest();

            var result = await _accountManager.Login(model);

            if (!result.isSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

       

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (String.IsNullOrEmpty(model.Username) || String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.Email))
            {
                return BadRequest();
            }
            
            var result = await _accountManager.RegisterAdmin(model);

            if (!result.isSuccess) { 
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}

