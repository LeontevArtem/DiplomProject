namespace WorkplacesAccounting.Classes
{
    public class Auditory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User ResponsibleUser { get; set; }
        public int ResponsibleUserId { get; set; }
        public bool isActive { get; set; }
        public int AmountOfSessions { get; set; }
        public List<Computer> computers { get; set; }
    }
}
