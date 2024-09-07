using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminPortal.Filters
{
    public class TokenAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenAuthorizationFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            

            if (string.IsNullOrEmpty(token))
            {
                // Redirect to login if token is missing
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
