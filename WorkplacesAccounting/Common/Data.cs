using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using WorkplacesAccounting.Controllers;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Common
{
    public static  class Data
    {
        public static Thread DataMonitoring;

        public static List<Models.Session> SessionsList;
        public static List<Models.User> UsersList;
        public static List<Models.LogData> LogList;
        public static List<Models.Auditory> AuditoryList;
        public static List<Models.Observation> ObservationsList;
        public static List<Models.Computer> ComputersList;
        public static string ConnectionString = "server = DESKTOP-OGA8BNV; Trusted_Connection = No; DataBase = Diplom; User = sa; PWD = sa";

        public static void StartDataMonitoringThread()
        {
            DataMonitoring = new Thread(() =>
            {
                while (true)
                {
                    LoadData();
                    Thread.Sleep(100);
                }
            });
            DataMonitoring.Start();
        }






        public static void LoadData()
        {
            //UsersList.Clear();
            //LogList.Clear();
            //AuditoryList.Clear();
            //SessionsList.Clear();
            //ObservationsList.Clear();
            UsersList = new List<Models.User>();
            System.Data.DataTable UserQuery = MsSQL.Query($"SELECT * FROM [dbo].[Users]", ConnectionString);
            for (int i = 0; i < UserQuery.Rows.Count; i++)
            {
                Models.User NewUser = new Models.User();
                NewUser.id = Convert.ToString(UserQuery.Rows[i][0]);
                NewUser.username = Convert.ToString(UserQuery.Rows[i][1]);
                NewUser.password = Convert.ToString(UserQuery.Rows[i][2]);
                NewUser.firstname = Convert.ToString(UserQuery.Rows[i][3]);
                NewUser.lastname = Convert.ToString(UserQuery.Rows[i][4]);
                NewUser.email = Convert.ToString(UserQuery.Rows[i][5]);
                NewUser.cohort = Convert.ToString(UserQuery.Rows[i][6]);
                if(!UsersList.Exists(x=>x.id==NewUser.id)) UsersList.Add(NewUser);

            }
            ComputersList = new List<Models.Computer>();
            System.Data.DataTable ComputerQuery = MsSQL.Query($"SELECT * FROM [dbo].[Computers]", ConnectionString);
            for (int i = 0; i < ComputerQuery.Rows.Count; i++)
            {
                Models.Computer NewComputer = new Models.Computer();
                NewComputer.Id = Convert.ToInt32(ComputerQuery.Rows[i][0]);
                NewComputer.UID = Convert.ToString(ComputerQuery.Rows[i][1]);
                NewComputer.IPAddress = Convert.ToString(ComputerQuery.Rows[i][2]);
                //NewComputer.Port= Convert.ToString(ComputerQuery.Rows[i][3]);
                NewComputer.MachineName = Convert.ToString(ComputerQuery.Rows[i][4]);
                if (!ComputersList.Exists(x => x.Id == NewComputer.Id)) ComputersList.Add(NewComputer);

            }

            AuditoryList = new List<Models.Auditory>();
            System.Data.DataTable AuditoryQuery = MsSQL.Query($"SELECT * FROM [dbo].[Auditory]", ConnectionString);
            for (int i = 0; i < AuditoryQuery.Rows.Count; i++)
            {
                Models.Auditory NewAuditory = new Models.Auditory();
                NewAuditory.Id = Convert.ToInt32(AuditoryQuery.Rows[i][0]);
                NewAuditory.Name = Convert.ToString(AuditoryQuery.Rows[i][1]);
                if(!AuditoryList.Exists(x=>x.Id==NewAuditory.Id)) AuditoryList.Add(NewAuditory);

            }

            SessionsList = new List<Models.Session>();
            System.Data.DataTable SessionQuery = MsSQL.Query($"SELECT * FROM [dbo].[Sessions]", ConnectionString);
            for (int i = 0; i < SessionQuery.Rows.Count; i++)
            {
                Models.Session NewSession = new Session();
                NewSession.ID = Convert.ToInt32(SessionQuery.Rows[i][0]);
                NewSession.User = UsersList.Find(x => x.id == Convert.ToString(SessionQuery.Rows[i][1]));
                NewSession.StartTime = Convert.ToString(SessionQuery.Rows[i][2]);
                NewSession.EndTime = Convert.ToString(SessionQuery.Rows[i][3]);
                NewSession.Auditory = AuditoryList.Find(x=>x.Id== Convert.ToInt32(SessionQuery.Rows[i][4]));
                NewSession.Computer = ComputersList.Find(x=>x.Id== Convert.ToInt32(SessionQuery.Rows[i][5]));
                NewSession.WorkareaPreview = Convert.ToString(SessionQuery.Rows[i][6]);
                if(!SessionsList.Exists(x=>x.ID==NewSession.ID))SessionsList.Add(NewSession);
            }

            LogList = new List<Models.LogData>();
            System.Data.DataTable LogQuery = MsSQL.Query($"SELECT * FROM [dbo].[Log]", ConnectionString);
            for (int i=0;i<LogQuery.Rows.Count;i++)
            {
                Models.LogData NewLog = new Models.LogData();
                NewLog.LogID = Convert.ToInt32(LogQuery.Rows[i][0]);
                NewLog.Session = SessionsList.Find(x =>x.ID == Convert.ToInt32(LogQuery.Rows[i][1]));
                NewLog.Data = Convert.ToString(LogQuery.Rows[i][2]);
                NewLog.Date = Convert.ToString(LogQuery.Rows[i][3]);
                NewLog.Tag = Convert.ToString(LogQuery.Rows[i][4]);
                if(!LogList.Exists(x=>x.LogID==NewLog.LogID))LogList.Add(NewLog);
            }

            ObservationsList = new List<Models.Observation>();
            System.Data.DataTable ObservationsQuery = MsSQL.Query($"SELECT * FROM [dbo].[Observations]", ConnectionString);
            for (int i = 0; i < ObservationsQuery.Rows.Count; i++)
            {
                Models.Observation NewObservation = new Models.Observation();
                NewObservation.Id = Convert.ToInt32(ObservationsQuery.Rows[i][0]);
                NewObservation.Data = Convert.ToString(ObservationsQuery.Rows[i][1]);
                NewObservation.Date = Convert.ToString(ObservationsQuery.Rows[i][2]);
                NewObservation.Session = SessionsList.Find(x=>x.ID== Convert.ToInt32(ObservationsQuery.Rows[i][3]));
                if(!ObservationsList.Exists(x=>x.Id==NewObservation.Id))ObservationsList.Add(NewObservation);
            }
        }
    }
}
