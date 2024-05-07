namespace WorkplacesAccounting.Models
{
    public class Observation
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string Date { get; set; }
        public Session Session { get; set; }

    }
}
