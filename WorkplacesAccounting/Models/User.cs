using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
    }

    
}
