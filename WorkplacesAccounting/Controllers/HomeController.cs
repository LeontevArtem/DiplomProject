using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Controllers
{
    public class HomeController : Controller
    {
       


        private readonly ILogger<HomeController> _logger;
        


        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index(string SearchString)
        {
            if (HttpContext.Session.GetString("UserGroup")!=null||true)//¬ будущем надо сделать так, чтобы доступ был только у преподователей. Ќу или как скажут.
            {
                Data.LoadData();
                Models.HomeModel model = new Models.HomeModel();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    model.SessionsList = Data.SessionsList.Where(x =>x.User.firstname.ToLower().Contains(SearchString)||Convert.ToString(x.ID).ToLower().Contains(SearchString)||x.ComputerName.ToLower().Contains(SearchString)).ToList();
                }
                else model.SessionsList = Data.SessionsList;

                return View(model);
            }
            return RedirectToAction("Index", "Authorization");
        }

        [Authorize]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
