using Microsoft.AspNetCore.Mvc;

namespace WorkplacesAccounting.Controllers
{
    public class StatisticController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
