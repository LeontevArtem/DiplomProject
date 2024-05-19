using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkplacesAccounting.Classes
{
    public class ProcessWindow
    {
        public string WindowTitle { get; private set; }
        public string StartTime { get; private set; }
        public string Time { get; set; }

        public ProcessWindow(string windowTitle,string startTime)
        {
            WindowTitle = windowTitle;
            StartTime = startTime;
        }
    }
}
