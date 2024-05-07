using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.Classes
{
    public class Observation
    {
        public int Id { get; set; }
        public string Data { get; set; }
        public string Date { get; set; }
        public Session Session { get; set; }

    }
}
