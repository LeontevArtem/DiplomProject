using Microsoft.AspNetCore.Mvc;
using WorkplacesAccounting.Common;

namespace WorkplacesAccounting.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            List<Classes.User> model = new List<Classes.User>();
            Data.Action(() =>
            {
                model = Data.UsersList;
            });
            return View(model);
        }
        public IActionResult UserInfo(string id)
        {
            Classes.User model = new Classes.User();
            Data.Action(() =>
            {
                model = Data.UsersList.ToList().Find(x => x.id.ToString() == id);
            });
            return View(model);
        }
    }
}
