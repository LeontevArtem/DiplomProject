using Microsoft.AspNetCore.Mvc;
using WorkplacesAccounting.Common;

namespace WorkplacesAccounting.Controllers
{
    public class AuditoryController : Controller
    {
        public IActionResult Auditories()
        {
            Models.AuditoriesListModel AuditoriesList = new Models.AuditoriesListModel();
            AuditoriesList.AuditoryExtended = new List<Models.AuditoryExtended>();
            foreach (Classes.Auditory curAuditory in Data.AuditoryList)
            {
                Models.AuditoryExtended auditoryExtended = new Models.AuditoryExtended();
                auditoryExtended.Auditory = curAuditory;
                auditoryExtended.Users = Data.SessionsList.Where(x => x.EndTime == "" && x.Auditory.Id == curAuditory.Id).Select(x => x.User).ToList();
                auditoryExtended.Auditory.AmountOfSessions = Data.SessionsList.Where(x => x.EndTime == "" && x.Auditory.Id == curAuditory.Id).Count();
                try
                {
                    auditoryExtended.workload = Convert.ToInt32(Convert.ToDouble(auditoryExtended.Users.Count()) / 30 * 100);
                }
                catch { }
                if (auditoryExtended.workload != 0) auditoryExtended.Auditory.isActive = true;

                AuditoriesList.AuditoryExtended.Add(auditoryExtended);
            }
            return View(AuditoriesList);
        }
        //public IActionResult Edit(Classes.Auditory auditory)
        //{
        //    return View();
        //}
        [HttpGet]
        public IActionResult Edit(string id)
        {
            return View(Data.AuditoryList.Find(x => x.Id.ToString() == id));
        }
        [HttpPost]
        public IActionResult Edit(Classes.Auditory auditory)
        {
            MsSQL.Query($"UPDATE [dbo].[Auditory] SET [Name] = '{auditory.Name}', [CurentResponsibleTeacher] = '{Data.UsersList.Find(x => x.id == auditory.ResponsibleUserId.ToString()).id}' WHERE ID = '{auditory.Id}' ", Data.ConnectionString);
            return RedirectToAction("Auditories", "Auditory");
        }
        [HttpGet]
        public IActionResult Add()
        {
            return RedirectToAction("Edit", "Auditory");
        }
        [HttpPost]
        public IActionResult Add(Classes.Auditory auditory)
        {
            System.Data.DataTable Insert = MsSQL.Query($"INSERT INTO [dbo].[Auditory]([Name],[CurentResponsibleTeacher]) VALUES ('{auditory.Name}','{Data.UsersList.Find(x => x.id == auditory.ResponsibleUserId.ToString()).id}')", Data.ConnectionString);
            return RedirectToAction("Auditories", "Auditory");
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            MsSQL.Query($"DELETE FROM [dbo].[Auditory] WHERE ID = '{id}'", Data.ConnectionString);
            return RedirectToAction("Auditories", "Auditory");
        }

    }
}
