using DataAccess.CommonModel.Response;
using DataAccess.DBContext;
using DataAccess.Models;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Account
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AccountRepo(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login(LoginModel model)
        {
            LoginResponse response = new LoginResponse();
            try
            {

                if (_configuration != null)
                {
                    var user = await userManager.FindByNameAsync(model.Username);
                    if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
                    {
                        var userRoles = await userManager.GetRolesAsync(user);

                        var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                        foreach (var userRole in userRoles)
                        {
                            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                        }

                        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                        var token = new JwtSecurityToken(
                            expires: DateTime.Now.AddHours(3),
                            claims: authClaims,
                            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                            );
                        response.token = new JwtSecurityTokenHandler().WriteToken(token);
                        response.expiration = token.ValidTo.ToString();
                        response.isSuccess = true;
                        response.message = "login success";
                        return response;
                    } 
                }
            }
            catch (Exception)
            {
                response.isSuccess = false;
                response.message = "Login failed, please try again or contact admin";
            }
            return response;
        }

        public async Task<CommonResponse> RegisterAdmin(RegisterModel model)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userExists = await userManager.FindByNameAsync(model.Username);
                if (userExists != null)
                    return  new CommonResponse { Status = "Error", Message = "User already exists!" };

                ApplicationUser user = new ApplicationUser()
                {
                    Email = model.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = model.Username
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                    return new CommonResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." };

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await userManager.AddToRoleAsync(user, UserRoles.Admin);
                }
                response.Status = "success";
                response.isSuccess = true;
                response.Message = "Admin user created";
                return response;
            }
            catch (Exception e)
            {
                response.Message = "Registration failed, please try again";
                response.Status = "failed";
            }
            return response;
        }
    }
}
