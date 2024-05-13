using System.Drawing;

namespace WorkplacesAccounting.Classes
{
    public class Session
    {
        public int ID { get; set; }
        public User User { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public Auditory Auditory { get; set; }
        public Computer Computer { get; set; }
        public string WorkareaPreview { get; set; }
    }
}
