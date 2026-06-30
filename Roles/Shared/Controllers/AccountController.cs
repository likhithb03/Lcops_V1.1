using Microsoft.AspNetCore.Mvc;

namespace LOCPS.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string role, string? returnUrl)
        {
            SetRoleCookie(role ?? "officer");
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(string role)
        {
            SetRoleCookie(role ?? "customer");
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("locps_demo_role");
            return RedirectToAction("Login");
        }

        private void SetRoleCookie(string role)
        {
            var normalized = role.ToLowerInvariant();
            if (normalized is not ("customer" or "officer" or "underwriter" or "admin"))
                normalized = "officer";

            Response.Cookies.Append("locps_demo_role", normalized, new CookieOptions
            {
                Path = "/",
                MaxAge = TimeSpan.FromDays(365),
                SameSite = SameSiteMode.Lax
            });
        }
    }
}
