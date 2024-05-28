using WorkplacesAccounting.Classes;

namespace WorkplacesAccounting.Models
{
    public class ReportModel
    {
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<ReportRow> reportRows { get; set; }
    }
    public class ReportRow
    {
        public Auditory Auditory { get; set; }
        public string PCNumber { get; set; }
        public string StudentName { get; set; }
        public string Observations { get; set; }
    }
}
