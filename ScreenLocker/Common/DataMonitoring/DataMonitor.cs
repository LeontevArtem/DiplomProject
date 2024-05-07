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
        
        public static List<string> GetAllProcesses()
        {
            //foreach (Process process in Process.GetProcesses().Where(p => p.MainWindowHandle != IntPtr.Zero && p.ProcessName != "explorer"))
            //{
            //    ProcessInfoList.Add(process.MainWindowTitle.ToString());
            //}
            //return ProcessInfoList;
            List<string> ProcessInfoList = new List<string>();
            ProcessWindow[] applications = ProcessHelper.GetRunningApplications();
            foreach (ProcessWindow pw in applications)
            {
                ProcessInfoList.Add(pw.WindowTitle);
            }
            return ProcessInfoList;
        }



    }
}
