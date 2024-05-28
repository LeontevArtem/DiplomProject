using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Observation,
            Message,
            ProcessKill
        }

        public void StartSession()
        {
            Computer computer = new Computer();
            computer.SaveToDataBase();
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Sessions] ([UserID], [StartTime],[Auditory],[Computer] )VALUES('{User.id}', '{DateTime.Now}','{Auditory.ID}','{computer.Id}'); SELECT SCOPE_IDENTITY();", MainWindow.ConnectionString);
            Id = Convert.ToInt32(Insert.Rows[Insert.Rows.Count - 1][0]);
            WriteLogToDataBase($"Сессия {Id} начата. Пользователь: {User.firstname}", Session.LogTag.Start);
        }
        public void EndSession()
        {
            Common.DataBase.MsSQL.Query($"UPDATE [dbo].[Sessions] SET [EndTime] = '{DateTime.Now}' WHERE SessionID = '{Id}' ", MainWindow.ConnectionString);
            WriteLogToDataBase($"Сессия {Id} завершена. Пользователь {User.firstname}", Session.LogTag.End);
        }
        public void AddObservation(string Data)
        {
            Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Observations]([Data],[Date],[SessionID])VALUES('{Data}','{DateTime.Now}','{Id}')", MainWindow.ConnectionString);
            WriteLogToDataBase($"Пользователем {User.firstname} добавлено замечание: {Data}", Session.LogTag.Observation);
        }

        public void WriteLogToDataBase(string Data, LogTag Tag)
        {
            var rx = new Regex(@"\\u([0-9A-Z]{4})", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Log]([SessionID],[Data],[Date],[Tag])VALUES('{Id}','{rx.Replace(Data, p => new string((char)int.Parse(p.Groups[1].Value, NumberStyles.HexNumber), 1))}','{DateTime.Now}','{Tag.ToString()}')", MainWindow.ConnectionString);
        }
        public bool CheckAccess()
        {
            System.Data.DataTable Check = Common.DataBase.MsSQL.Query($"SELECT [EndTime] FROM [dbo].[Sessions] WHERE SessionID = '{Id}' ", MainWindow.ConnectionString);
            DateTime EndTime = new DateTime();
            return DateTime.TryParse(Check.Rows[Check.Rows.Count-1][0].ToString(), out EndTime);
            //return (EndTime != null);
        }
        public void CheckMessage()
        {
            System.Data.DataTable Message = Common.DataBase.MsSQL.Query($"SELECT MessageID,FromID, MessageText,IsRead,Tag FROM [dbo].[Message] WHERE ToID = {User.id}", MainWindow.ConnectionString);
            for (int i=0;i< Message.Rows.Count;i++)
            {
                Common.Classes.Message message = new Message();
                message.ID = Convert.ToInt32(Message.Rows[i][0]);
                message.From = MainWindow.users.Find(x=>x.id== Convert.ToString(Message.Rows[i][1]));
                message.MessageText = Convert.ToString(Message.Rows[i][2]);
                message.IsRead = Convert.ToInt32(Message.Rows[i][3])==1?true:false;
                message.Tag = Convert.ToString(Message.Rows[i][4]);
                if (!message.IsRead)
                {
                    if (message.Tag == "Message")
                    {
                        MessageBox.Show($"{message.MessageText}", $"Сообщение от {message.From.firstname}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataBase.MsSQL.Query($"UPDATE [dbo].[Message] SET [IsRead] = {1} WHERE MessageID = {message.ID}", MainWindow.ConnectionString);
                        WriteLogToDataBase($"Получено сообщение от пользователя {message.From.firstname}",LogTag.Message);
                    }
                    if (message.Tag == "ProcessKill")
                    {
                        //MessageBox.Show($"{message.MessageText}", $"Сообщение от {message.From.firstname}", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DataBase.MsSQL.Query($"UPDATE [dbo].[Message] SET [IsRead] = {1} WHERE MessageID = {message.ID}", MainWindow.ConnectionString);
                        try
                        {
                            Process process = Process.GetProcessById(Convert.ToInt32(message.MessageText));
                            process.Kill();
                            WriteLogToDataBase("", LogTag.ProcessKill);
                        }
                        catch { }
                        
                    }
                }
                
            }


        }
    }
}
