using Microsoft.AspNetCore.Mvc;
using WorkplacesAccounting.Common;

namespace WorkplacesAccounting.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View(Data.UsersList);
        }
    }
}
