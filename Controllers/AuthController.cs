using _220008504_AuthBasics.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _220008504_AuthBasics.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginFormModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var account = UserManager.Login(model.UserName, model.Password);

            if (account == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

           var identity = new ClaimsIdentity(account.Claims, Settings.AuthCookieName);
           var principal = new ClaimsPrincipal(identity);

            AuthenticationProperties props;

            if(model.RememberMe)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                    AllowRefresh = true,
                };
            }
            else
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = false,
                    AllowRefresh = true,
                };
            }

            await HttpContext.SignInAsync(Settings.AuthCookieName, principal,props);

            HttpContext.Session.SetString("UserName", model.UserName);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Settings.AuthCookieName);
            return RedirectToAction("Index", "Home");
        }


    }
}
