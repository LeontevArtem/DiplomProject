using Microsoft.AspNetCore.Mvc;
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
            if (ModelState.IsValid)
            {
                User user = Models.User.ConvertJsonToUser(await Common.Moodle.Authenticate(model.Login,model.Password));
                if (user.Validate()) 
                {
                    user.SaveToDatabase();
                    HttpContext.Session.SetString("UserId", user.id);
                    HttpContext.Session.SetString("UserGroup", user.cohort);
                    return RedirectToAction("Index", "Home"); 
                }

            }
            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }
    }
}
