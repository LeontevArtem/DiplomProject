using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ScreenLocker.Common.DataBase
{
    public class MsSQL
    {
        public static DataTable Query(string selectSQL, string connectionString)
        {
            DataTable dataTable = new DataTable("database");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = selectSQL;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }
        public static void WriteLogToDataBase(int SessionID,string Data)
        {
            System.Data.DataTable Insert = Common.DataBase.MsSQL.Query($"INSERT INTO [dbo].[Log]([SessionID],[Data])VALUES('{SessionID}','{Data}')", MainWindow.ConnectionString);
        }
    }
}
