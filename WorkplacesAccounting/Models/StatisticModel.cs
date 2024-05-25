using WorkplacesAccounting.Classes;

namespace WorkplacesAccounting.Models
{
    public class StatisticModel
    {
        public int AmountOfUsers { get; set; }
        public int UsersOnline { get; set; }
        public double AverageSessionTimespan { get; set; }
        public string MostPopularProgramm { get; set; }
        public List<ProcessWindow> processWindows { get; set; }
    }
}
