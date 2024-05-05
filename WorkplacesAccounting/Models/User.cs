using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WorkplacesAccounting.Common;

namespace WorkplacesAccounting.Models
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




        public static User ConvertJsonToUser(string UserJson)
        {
            User user = new User();
            try
            {

                user = JsonSerializer.Deserialize<User>(UserJson);

            }
            catch { }
            return user;
        }
        public bool Validate()
        {
            return id != null;
        }
        public void SaveToDatabase()
        {
            System.Data.DataTable Check = MsSQL.Query($"SELECT * FROM [dbo].[Users] WHERE Username='{username}' AND Password='{password}'", Data.ConnectionString);
            if (Check.Rows.Count == 0)
            {
                System.Data.DataTable Insert = MsSQL.Query($"INSERT INTO [dbo].[Users]([UserID],[Username],[Password],[Firstname],[Lastname],[email],[Groups]) VALUES('{id}','{username}','{password}','{firstname}','{lastname}','{email}','{cohort}') ", Data.ConnectionString);
            }
        }
    }

    
}
