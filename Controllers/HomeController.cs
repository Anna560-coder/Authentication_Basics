using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _220008504_AuthBasics.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("UserName");
            ViewBag.UserName = username;

            return View();
        }

        [Authorize]
        public IActionResult UsersOnly()
        {
            return View();
        }

        [Authorize("Admin")]
        public IActionResult AdminOnly()
        {
            return View();
        }
    }
}
