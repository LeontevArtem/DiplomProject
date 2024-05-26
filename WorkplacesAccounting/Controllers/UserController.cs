using Microsoft.AspNetCore.Mvc;
using WorkplacesAccounting.Common;

namespace WorkplacesAccounting.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            Data.LoadData();
            return View(Data.UsersList);
        }
        public IActionResult UserInfo(string id)
        {
            Data.LoadData();
            return View(Data.UsersList.ToList().Find(x=>x.id.ToString()==id));
        }
    }
}
