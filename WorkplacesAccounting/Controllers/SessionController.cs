﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using WorkplacesAccounting.Common;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Controllers
{
    public class SessionController : Controller
    {
        [Authorize]
        public IActionResult Index(string id)
        {
            //Data.LoadData();
            SessionModel model = new SessionModel();
            //Data.LoadData();
            model.Session = Data.SessionsList.Find(x => x.ID==Convert.ToInt32(id));
            model.Logs = Data.LogList.Where(x => x.Session==model.Session).ToList();
            model.ObservationsList = Data.ObservationsList.Where(x =>x.Session.ID==model.Session.ID).ToList();

            return View(model);
        }
        [Authorize]
        public IActionResult Test(string id)
        {
            //Debug.WriteLine(id
            //Response.WriteAsync("<script>alert('Your text');</script>");
            //Session curSession = Data.SessionsList.Find(x =>x.ID.ToString()==id);
            System.Data.DataTable Insert = MsSQL.Query($"UPDATE [dbo].[Sessions] SET [EndTime] = '{DateTime.Now}' WHERE SessionID = '{id}' ", Data.ConnectionString);

            return RedirectToAction("Index", new { id });
        }
    }
}
