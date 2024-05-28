using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            Data.Action(() =>
            {
                model.Session = Data.SessionsList.Find(x => x.ID == Convert.ToInt32(id));
                model.Logs = Data.LogList.Where(x => x.Session == model.Session).ToList();
                model.ObservationsList = Data.ObservationsList.Where(x => x.Session.ID == model.Session.ID).ToList();
                model.Programms = new List<ProcessWindow>();
                if (model.Session.EndTime == "")
                {
                    Classes.LogData logData = model.Logs.Where(x => x.Tag == "Processes" && x.Session.ID == model.Session.ID).LastOrDefault();
                    if (logData!=null)
                    {
                        model.Programms = JsonSerializer.Deserialize<List<ProcessWindow>>(logData.Data);
                    }

                }
                else
                {
                    foreach (LogData ProgrammInfo in model.Logs.Where(x => x.Tag == "Processes" && x.Session.ID == model.Session.ID))
                    {
                        try
                        {
                            List<ProcessWindow> processWindows = JsonSerializer.Deserialize<List<ProcessWindow>>(ProgrammInfo.Data);
                            foreach (ProcessWindow processWindow in processWindows) processWindow.Time = ProgrammInfo.Date;
                            model.Programms.AddRange(processWindows);//
                        }
                        catch { }

                    }
                }
            });
            return View(model);
        }
        [Authorize]
        public IActionResult EndSession(string id)
        {
            System.Data.DataTable Insert = MsSQL.Query($"UPDATE [dbo].[Sessions] SET [EndTime] = '{DateTime.Now}' WHERE SessionID = '{id}' ", Data.ConnectionString);
            return RedirectToAction("Index", new { id });
        }
        [Authorize]
        [HttpGet]
        public IActionResult Message(string id)
        {
            //string Userid = Data.SessionsList.ToList().Find(x => x.ID == Convert.ToInt32(id)).User.id;
            //System.Data.DataTable Insert = MsSQL.Query($"INSERT INTO [dbo].[Message]([FromID],[ToID],[MessageText],[Tag])VALUES('{Data.CurrentUser.id}','{Userid}','{"Test"}','Message')", Data.ConnectionString);
            //return RedirectToAction("Index", new { id });
            Classes.Message message = new Message();
            message.ID = Convert.ToInt32(id);
            Data.Action(() =>
            {
                //messages = Data.MessagesList.Where(x=>x.To.id == Data.SessionsList.Find(x=>x.ID.ToString()==id).User.id).ToList();
                message.From.id = Data.CurrentUser.id;
                message.To.id = Data.SessionsList.ToList().Find(x => x.ID == Convert.ToInt32(id)).User.id;
            });
            return View(message);
        }
        [Authorize]
        [HttpPost]
        public IActionResult SendMessage(Classes.Message message)
        {
            MsSQL.Query($"INSERT INTO [dbo].[Message]([FromID],[ToID],[MessageText],[Tag],[Date])VALUES('{message.From.id}','{message.To.id}','{message.MessageText}','Message','{DateTime.Now}')", Data.ConnectionString);
            return RedirectToAction("Message", new { id = message.ID});
        }



        [Authorize]
        public IActionResult KillProcess(string ProcessId, string SessionId)
        {
            string id = SessionId;
            string Userid = Data.SessionsList.ToList().Find(x => x.ID == Convert.ToInt32(SessionId)).User.id;
            System.Data.DataTable Insert = MsSQL.Query($"INSERT INTO [dbo].[Message]([FromID],[ToID],[MessageText],[Tag])VALUES('{Data.CurrentUser.id}','{Userid}','{ProcessId}','ProcessKill')", Data.ConnectionString);
            return RedirectToAction("Index", new { id });
        }
    }
}
