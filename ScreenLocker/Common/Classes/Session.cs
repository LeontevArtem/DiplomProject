using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScreenLocker.Common.Classes
{
    public class Session
    {
        public int Id { get; set; }
        public User User { get; set; }
        public System.Diagnostics.Process[] Processes;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Auditory Auditory { get; set; }
        public string ComputerName {  get; set; }

        public enum LogTag
        {
            Start,
            Processes,
            End,
            Observation
        }

        public void StartSession()
        {
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Sessions] ([UserID], [StartTime],[Auditory],[Computer] )VALUES('{User.id}', '{DateTime.Now}','{Auditory.ID}','{System.Environment.MachineName}'); SELECT SCOPE_IDENTITY();", MainWindow.ConnectionString);
            Id = Convert.ToInt32(Insert.Rows[Insert.Rows.Count - 1][0]);
            WriteLogToDataBase($"Сессия {Id} начата. Пользователь: {User.firstname}", Session.LogTag.Start);
        }
        public void EndSession()
        {
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"UPDATE [dbo].[Sessions] SET [EndTime] = '{DateTime.Now}' WHERE SessionID = '{Id}' ", MainWindow.ConnectionString);
            WriteLogToDataBase($"Сессия {Id} завершена. Пользователь {User.firstname}", Session.LogTag.End);
        }
        public void AddObservation(string Data)
        {
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Observations]([Data],[Date],[SessionID])VALUES('{Data}','{DateTime.Now}','{Id}')", MainWindow.ConnectionString);
            WriteLogToDataBase($"Пользователем {User.firstname} добавлено замечание: {Data}", Session.LogTag.Observation);
        }

        public void WriteLogToDataBase(string Data, LogTag Tag)
        {
            var rx = new Regex(@"\\u([0-9A-Z]{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Log]([SessionID],[Data],[Date],[Tag])VALUES('{Id}','{rx.Replace(Data, p => new string((char)int.Parse(p.Groups[1].Value, NumberStyles.HexNumber), 1))}','{DateTime.Now}','{Tag.ToString()}'); SELECT SCOPE_IDENTITY()", MainWindow.ConnectionString);
            //int test = Convert.ToInt32(Insert.Rows[Insert.Rows.Count - 1][0]);
        }

    }
}
