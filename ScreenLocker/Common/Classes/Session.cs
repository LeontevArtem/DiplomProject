using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.Classes
{
    public class Session
    {
        public int Id { get; set; }
        public User User { get; set; }
        public System.Diagnostics.Process[] Processes;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
