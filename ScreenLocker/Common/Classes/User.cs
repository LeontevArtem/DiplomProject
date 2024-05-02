using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.Classes
{
    public class User
    {
        public string id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string cohort { get; set; }

        public int SessionID {  get; set; }


        public bool Validate()
        {
            return id != null;
        }
        public void SaveToDatabase()
        {
            System.Data.DataTable Check = Common.DataBase.MsSQL.Query($"SELECT * FROM [dbo].[Users] WHERE Username='{username}' AND Password='{password}'", MainWindow.ConnectionString);
            if (Check.Rows.Count==0) 
            {
                System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Users]([UserID],[Username],[Password],[Firstname],[Lastname],[email],[Groups]) VALUES('{id}','{username}','{password}','{firstname}','{lastname}','{email}','{cohort}') ", MainWindow.ConnectionString);
            }
        }
        public void StartSession()
        {
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO[dbo].[Sessions] ([UserID], [StartTime], [Observations])VALUES('{id}', '{DateTime.Now}', 'NULL'); SELECT SCOPE_IDENTITY();", MainWindow.ConnectionString);
            SessionID = Convert.ToInt32(Insert.Rows[Insert.Rows.Count-1][0]);

        }
    }
}
