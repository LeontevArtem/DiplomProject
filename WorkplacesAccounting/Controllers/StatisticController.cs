using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WorkplacesAccounting.Classes;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Controllers
{
    public class StatisticController : Controller
    {
        public IActionResult Index()
        {
            StatisticModel model = new StatisticModel();
            Data.Action(() =>{
                model.AmountOfUsers = Data.UsersList.Count;
                model.UsersOnline = Data.SessionsList.Where(x => x.EndTime == "").ToList().Count;

                foreach (Session session in Data.SessionsList.Where(x => x.EndTime != "").ToList())
                {
                    DateTime StartTime = DateTime.Parse(session.StartTime);
                    DateTime EndTime = DateTime.Parse(session.EndTime);
                    TimeSpan timeSpan = EndTime.Subtract(StartTime);
                    double Hours = timeSpan.TotalHours;
                    model.AverageSessionTimespan += Hours;
                }
                model.AverageSessionTimespan /= Data.SessionsList.Where(x => x.EndTime != "").ToList().Count;
                model.AverageSessionTimespan = Math.Round(model.AverageSessionTimespan, 2);
                List<ProcessWindow> programs = new List<ProcessWindow>();
                try
                {
                    foreach (LogData ProgrammInfo in Data.LogList.Where(x => x.Tag == "Processes"))
                    {
                        try
                        {
                            List<ProcessWindow> processWindows = JsonSerializer.Deserialize<List<ProcessWindow>>(ProgrammInfo.Data);
                            foreach (ProcessWindow processWindow in processWindows) processWindow.Time = ProgrammInfo.Date;
                            programs.AddRange(processWindows);
                        }
                        catch { }

                    }
                }
                catch (Exception ex)
                {

                }
                model.processWindows = programs;
            });
            return View(model);
        }
    }
}
