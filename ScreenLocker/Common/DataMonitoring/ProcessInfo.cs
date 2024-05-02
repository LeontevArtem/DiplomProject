using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.DataMonitoring
{
    public class ProcessInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string User { get; set; }
        public string UpTime { get; set; }
    }
}
