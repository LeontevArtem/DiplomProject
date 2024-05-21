using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Diagnostics;
using System.Text.Json;
using WorkplacesAccounting.Classes;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Controllers
{
    public class SessionController : Controller
    {
        [Authorize]
        public IActionResult Index(string id)
        {
            SessionModel model = new SessionModel();
            model.Session = Data.SessionsList.Find(x => x.ID==Convert.ToInt32(id));
            model.Logs = Data.LogList.Where(x => x.Session==model.Session).ToList();
            model.ObservationsList = Data.ObservationsList.Where(x =>x.Session.ID==model.Session.ID).ToList();
            model.Programms = new List<ProcessWindow>();
            foreach (LogData ProgrammInfo in model.Logs.Where(x=>x.Tag=="Processes"&&x.Session.ID== model.Session.ID))
            {
                try
                {
                    List<ProcessWindow> processWindows = JsonSerializer.Deserialize<List<ProcessWindow>>(ProgrammInfo.Data);
                    foreach (ProcessWindow processWindow in processWindows) processWindow.Time = ProgrammInfo.Date;
                    model.Programms.AddRange(processWindows);
                }
                catch { }
                
            }
            return View(model);
        }
        [Authorize]
        public IActionResult EndSession(string id)
        {
            System.Data.DataTable Insert = MsSQL.Query($"UPDATE [dbo].[Sessions] SET [EndTime] = '{DateTime.Now}' WHERE SessionID = '{id}' ", Data.ConnectionString);

            return RedirectToAction("Index", new { id });
        }
    }
}
