using AdminPortal.Filters;
using AdminPortal.Models;
using DataAccess.CommonModel.Response;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace AdminPortal.Controllers
{
    [ServiceFilter(typeof(TokenAuthorizationFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppSettings _appSettings;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IOptionsMonitor<AppSettings> appSettings)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings.CurrentValue;
        }

        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = new HttpClient();
            List<CustomerModel> customers = new List<CustomerModel>();
            try
            {
                var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await httpClient.GetAsync(_appSettings.APIBaseURL + "/api/Customer/getCustomers");
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var getCustomersResponse = JsonConvert.DeserializeObject<CustomerListResponse>(stringResponse);

                    if (getCustomersResponse != null && getCustomersResponse.isSuccess)
                    {
                        return View(getCustomersResponse.CustomerList);
                    }
                }

            }
            catch (Exception)
            {

            }
            return View(customers);
        }

        public IActionResult Create() {
            CustomerModel customer = new CustomerModel();
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                try
                {
                    var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    var createResponse = await httpClient.PostAsJsonAsync(_appSettings.APIBaseURL+ "/api/Customer/createCustomer", model);

                    if (createResponse.IsSuccessStatusCode)
                    {
                        var stringContent = await createResponse.Content.ReadAsStringAsync();

                        var createResponseObject = JsonConvert.DeserializeObject<CommonResponse>(stringContent);
                        // Store token securely
                        if (!createResponseObject.isSuccess)
                        {
                            ModelState.AddModelError(string.Empty, createResponseObject.Message);
                        }

                        // Alternatively, you can store it in cookies (use HttpOnly for security)
                        // Response.Cookies.Append("JWToken", token, new CookieOptions { HttpOnly = true });

                        
                    }
                    else
                    {
                        // Handle login failure
                        ModelState.AddModelError(string.Empty, "Create request failed");
                        return RedirectToAction("Create", "Home",model);
                    }
                }
                catch (Exception)
                {

                    ModelState.AddModelError(string.Empty, "Create request failed");
                    return RedirectToAction("Create", "Home", model);
                }
                

            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int id)
        {
            HttpClient httpClient = new HttpClient();
            CustomerModel customer = new CustomerModel();
            try
            {
                var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await httpClient.GetAsync(_appSettings.APIBaseURL+ "/api/Customer/getCustomerById?id="+id);
                if (response.IsSuccessStatusCode)
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    var getCustomersResponse = JsonConvert.DeserializeObject<CustomerListResponse>(stringResponse);

                    if (getCustomersResponse != null && getCustomersResponse.isSuccess)
                    {
                        return View(getCustomersResponse.CustomerList.First());
                    }
                }

            }
            catch (Exception)
            {

            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CustomerModel model)
        {
            if (ModelState.IsValid)
            {
                HttpClient httpClient = new HttpClient();
                try
                {
                    var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                    if (!string.IsNullOrEmpty(token))
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    var editResponse = await httpClient.PutAsJsonAsync(_appSettings.APIBaseURL+ "/api/Customer/editCustomer", model);

                    if (editResponse.IsSuccessStatusCode)
                    {
                        var stringContent = await editResponse.Content.ReadAsStringAsync();

                        var createResponseObject = JsonConvert.DeserializeObject<CommonResponse>(stringContent);
                        // Store token securely
                        if (!createResponseObject.isSuccess)
                        {
                            ModelState.AddModelError(string.Empty, createResponseObject.Message);
                        }


                    }
                    else
                    {
                        // Handle login failure
                        ModelState.AddModelError(string.Empty, "Edit request failed");
                        return RedirectToAction("Edit", "Home", model);
                    }
                }
                catch (Exception)
                {

                    ModelState.AddModelError(string.Empty, "Edit request failed");
                    return RedirectToAction("Edit", "Home", model);
                }


            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await httpClient.DeleteAsync(_appSettings.APIBaseURL+ "/api/Customer/deleteCustomer?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    var stringContent = await response.Content.ReadAsStringAsync();

                    var deleteResponseObj = JsonConvert.DeserializeObject<CommonResponse>(stringContent);
                    // Store token securely
                    if (!deleteResponseObj.isSuccess)
                    {
                        ModelState.AddModelError(string.Empty, deleteResponseObj.Message);
                    }


                }
                else
                {
                    // Handle login failure
                    ModelState.AddModelError(string.Empty, "Delete request failed");
                    return RedirectToAction("Delete", "Home", new {id = id});
                }
            }
            catch (Exception)
            {

                throw;
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
