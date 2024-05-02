using Microsoft.VisualBasic.Logging;
using ScreenLocker.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace ScreenLocker.Common.Moodle
{
    
    public static class Moodle
    {
        public static async Task<string> Authenticate(string Login,string Password)
        {
            HttpClient HttpClient = new HttpClient();
            HttpResponseMessage response = await HttpClient.GetAsync($"https://edu.permaviat.ru/api/auth.php?login={Login}&password={Password}&token=e661c8f8398e613e48c41e26d9e64af1");
            return await response.Content.ReadAsStringAsync();
        }
        public static User ConvertJsonToUser(string UserJson)
        {
            User user = new User();
            try
            {
                
                user = JsonSerializer.Deserialize<User>(UserJson);
                
            }
            catch{ }
            return user;
        }
    }
}
