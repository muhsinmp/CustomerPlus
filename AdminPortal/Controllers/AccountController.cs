using DataAccess.CommonModel.Response;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AdminPortal.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;

        public AccountController(IOptionsMonitor<AppSettings> appSettings)
        {

            _appSettings = appSettings.CurrentValue;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpClient httpClient = new HttpClient();
                    var loginResponse = await httpClient.PostAsJsonAsync(_appSettings.APIBaseURL+ "/api/Authenticate/login", model);

                    if (loginResponse.IsSuccessStatusCode)
                    {
                        var token = await loginResponse.Content.ReadAsStringAsync();

                        var loginResponseObject = JsonConvert.DeserializeObject<LoginResponse>(token);
                        // Store token securely
                        HttpContext.Session.SetString("JWToken", loginResponseObject.token);


                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Handle login failure
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    }
                }
                catch (Exception e)
                {

                    ModelState.AddModelError(string.Empty, "Login Failure");
                    return RedirectToAction("Login", "Account");
                }
            }

            return View(model);
        }

    }
}
