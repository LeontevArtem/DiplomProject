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
        public string PCNumber { get; set; }
        public string StudentName { get; set; }
        public string Observations { get; set; }
    }
}
