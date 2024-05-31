using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenLocker.Common.Classes
{
    public class Message
    {
        public int ID { get; set; }
        public User From {  get; set; }
        public User To { get; set; }
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
        public string Tag { get; set; }
        public string Date { get; set; }
    }
}
