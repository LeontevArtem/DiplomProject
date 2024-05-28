using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.DataMonitoring
{
    public class ProcessWindow
    {
        public int PID {  get; set; }
        public string WindowTitle { get; private set; }
        //public Process Process { get; private set; }
        public string StartTime { get; private set; }

        public ProcessWindow(int PID,string windowTitle, Process process)
        {
            this.PID = PID;
            WindowTitle = windowTitle;
            //Process = process;
            StartTime = process.StartTime.ToString();
        }
    }
}
