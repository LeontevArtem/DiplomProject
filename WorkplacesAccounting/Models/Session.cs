namespace WorkplacesAccounting.Models
{
    public class Session
    {
        public int ID { get; set; }
        public User User {  get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Observation { get; set; }
        public Auditory Auditory { get; set; }
    }
}
