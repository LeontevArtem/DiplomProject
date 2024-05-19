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
        
        public static List<ProcessWindow> GetAllProcesses()
        {

            List<ProcessWindow> applications = ProcessHelper.GetRunningApplications();
            return applications;
        }
        


    }
    public class ProcessWindowSerializable
    {
        
    }
}
