using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WorkplacesAccounting.Classes;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(AuthModel model)
        {
            Data.LoadData();
            if (ModelState.IsValid)
            {
                User user = Classes.User.ConvertJsonToUser(await Common.Moodle.Authenticate(model.Login,model.Password));
                if (user.Validate()) 
                {
                    var claims = new List<Claim>
                        {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, user.username)
                        };
                    ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
                    user.SaveToDatabase();
                    HttpContext.Session.SetString("UserId", user.id);
                    HttpContext.Session.SetString("UserName", user.firstname);
                    HttpContext.Session.SetString("UserGroup", user.cohort);
                    Data.StartDataMonitoringThread();
                    Data.CurrentUser = user;
                    return RedirectToAction("Index", "Home"); 
                }

            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }
    }
}
