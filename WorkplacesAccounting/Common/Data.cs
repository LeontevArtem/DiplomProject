using WorkplacesAccounting.Controllers;
using WorkplacesAccounting.Models;

namespace WorkplacesAccounting.Common
{
    public static  class Data
    {

        public static List<Models.Session> SessionsList;
        public static List<Models.User> UsersList;
        public static List<Models.LogData> LogList;
        public static string ConnectionString = "server = DESKTOP-ARTEM; Trusted_Connection = No; DataBase = Diplom; User = sa; PWD = sa";

        public static void LoadData()
        {
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
                UsersList.Add(NewUser);
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
                NewSession.Observation = Convert.ToString(SessionQuery.Rows[i][4]);
                NewSession.Cabinet = Convert.ToString(SessionQuery.Rows[i][5]);
                SessionsList.Add(NewSession);
            }

            LogList = new List<Models.LogData>();
            System.Data.DataTable LogQuery = MsSQL.Query($"SELECT * FROM [dbo].[Log]", ConnectionString);
            for (int i=0;i<LogQuery.Rows.Count;i++)
            {
                Models.LogData NewLog = new Models.LogData();
                NewLog.LogID = Convert.ToInt32(LogQuery.Rows[i][0]);
                NewLog.Session = SessionsList.Find(x =>x.ID == Convert.ToInt32(LogQuery.Rows[i][1]));
                NewLog.Data = Convert.ToString(LogQuery.Rows[i][2]);
                LogList.Add(NewLog);
            }
        }
    }
}
