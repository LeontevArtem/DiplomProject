namespace WorkplacesAccounting.Models
{
    public class AuditoriesListModel
    {
        public int Id { get; set; }
        public List<AuditoryExtended> AuditoryExtended;
    }
    public class AuditoryExtended
    {
        public Classes.Auditory Auditory { get; set; }
        public List<Classes.User> Users { get; set; }
        public int workload { get; set; } = 0;
    }
}
