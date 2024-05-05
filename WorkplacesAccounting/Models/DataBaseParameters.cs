using Microsoft.AspNetCore.Mvc;

namespace WorkplacesAccounting.Models
{
    public class DataBaseParameters
    {
        public string Server {  get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ReturnConnectionString()
        {
            IConfiguration appConfig;
            return $"server = {Server}; Trusted_Connection = No; DataBase = {Database}; User = {Username}; PWD = {Password}";
        }
    }
}
