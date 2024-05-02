using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Principal;

namespace ScreenLocker.Common.DataMonitoring
{
    public class DataMonitor
    {
        
        public List<string> ProcessInfoList = new List<string>();
        public List<string> GetAllProcesses()
        {
            foreach (Process process in Process.GetProcesses())
            {
                ProcessInfoList.Add(process.ProcessName.ToString());
            }
            return ProcessInfoList;
        }

    }
}
