using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WorkplacesAccounting.Controllers;

namespace WorkplacesAccounting.Common
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
        public static void WriteLogToDataBase(int SessionID, string Info)
        {
            System.Data.DataTable Insert = MsSQL.Query($"INSERT INTO [dbo].[Log]([SessionID],[Data])VALUES('{SessionID}','{Info}')", Data.ConnectionString);
        }
    }
}
