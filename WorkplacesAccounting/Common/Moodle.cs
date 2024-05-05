using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace WorkplacesAccounting.Common
{
    public static class Moodle
    {
        public static async Task<string> Authenticate(string Login, string Password)
        {
            HttpClient HttpClient = new HttpClient();
            HttpResponseMessage response = await HttpClient.GetAsync($"https://edu.permaviat.ru/api/auth.php?login={Login}&password={Password}&token=e661c8f8398e613e48c41e26d9e64af1");
            return await response.Content.ReadAsStringAsync();
        }
    }
}
