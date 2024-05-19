using DeviceId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ScreenLocker.Common.Classes
{
    public class Computer
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string IPAddress {  get; set; }
        public string MachineName { get; set; }
        public Computer() 
        {
            UID = new DeviceIdBuilder().AddMachineName().AddMacAddress().ToString();
            MachineName = System.Environment.MachineName;
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress iPAddress in localIPs)
            {
                if(iPAddress.AddressFamily== System.Net.Sockets.AddressFamily.InterNetwork) IPAddress = iPAddress.ToString();
            }
        }
        public void SaveToDataBase()
        {
            System.Data.DataTable Check = Common.DataBase.MsSQL.Query($"SELECT [DBId] FROM [dbo].[Computers] WHERE UID='{UID}'", MainWindow.ConnectionString);
            if (Check.Rows.Count == 0)
            {
                System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Computers] ([UID],[MachineName],[IPAdress],[Port] )VALUES( '{UID}','{MachineName}','{this.IPAddress}','{0}'); SELECT SCOPE_IDENTITY();", MainWindow.ConnectionString);
                Id = Convert.ToInt32(Insert.Rows[Insert.Rows.Count - 1][0]);
            }
            else
            {
                Id = Convert.ToInt32(Check.Rows[Check.Rows.Count - 1][0]);
            }

            
        }

    }
}
