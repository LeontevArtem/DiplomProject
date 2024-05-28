namespace WorkplacesAccounting.Classes
{
    public class Message
    {
        public int ID { get; set; }
        public User From { get; set; } = new User();
        public User To { get; set; } = new User();
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public string Tag { get; set; }
        public string Date { get; set; }
    }
}
