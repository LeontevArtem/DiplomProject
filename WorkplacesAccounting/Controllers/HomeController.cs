using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Session;
using System.Diagnostics;
using WorkplacesAccounting.Classes;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Models;
using Session = WorkplacesAccounting.Classes.Session;

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
            Data.LoadData();
            if (HttpContext.Session.GetString("UserGroup")!=null||true)//¬ будущем надо сделать так, чтобы доступ был только у преподователей. Ќу или как скажут.
            {
                if(Data.SessionsList.Count==0) Data.LoadData();
                Models.HomeModel model = new Models.HomeModel();
                if (!String.IsNullOrEmpty(SearchString))
                {
                    model.SessionsList = Data.SessionsList.Where(x =>x.User.firstname.ToLower().Contains(SearchString.ToLower())|| x.User.lastname.ToLower().Contains(SearchString.ToLower())|| x.Auditory.Name.ToLower().Contains(SearchString.ToLower()) || Convert.ToString(x.ID).ToLower().Contains(SearchString.ToLower())||x.Computer.MachineName.ToLower().Contains(SearchString.ToLower())).OrderByDescending(x=>x.StartTime).ToList();
                }
                else model.SessionsList = Data.SessionsList.OrderByDescending(x => x.StartTime).ToList();

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
        [HttpGet]
        [Authorize]
        public IActionResult Report()
        {
            Data.LoadData();
            return View();
        }
        [HttpPost]
        public IActionResult Report(Models.ReportModel reportModel)
        {
            Data.LoadData();
            try
            {
                string[] StartDateMas = reportModel.StartTime.Split('/');
                DateTime StartDate = DateTime.Parse($"{StartDateMas[1]}.{StartDateMas[0]}.{StartDateMas[2]}");
                string[] EndDateMas = reportModel.EndTime.Split('/');
                DateTime EndDate = DateTime.Parse($"{EndDateMas[1]}.{EndDateMas[0]}.{EndDateMas[2]}");
                reportModel.reportRows = new List<ReportRow>();
                foreach (Session session in Data.SessionsList.Where(x => DateTime.Parse(x.StartTime).Ticks - StartDate.Ticks >= 0 && DateTime.Parse(x.EndTime).Ticks - EndDate.Ticks <= 0))
                {
                    long test1 = DateTime.Parse(session.StartTime).Ticks - StartDate.Ticks;
                    long test2 = DateTime.Parse(session.EndTime).Ticks - EndDate.Ticks;
                    ReportRow reportRow = new ReportRow();
                    reportRow.StudentName = $"{session.User.firstname} {session.User.lastname}";
                    reportRow.PCNumber = session.Computer.MachineName;
                    reportRow.Observations = "";

                    foreach (Observation observation in Data.ObservationsList.Where(x => x.Session.ID == session.ID))
                    {
                        reportRow.Observations += $"{observation.Data}. ";
                    }
                    reportModel.reportRows.Add(reportRow);
                }
                reportModel.StartTime = StartDate.ToString();
                reportModel.EndTime = EndDate.ToString();
            }
            catch { reportModel = null; }
            
            return View(reportModel);
        }


    }
}
