﻿using WorkplacesAccounting.Classes;

namespace WorkplacesAccounting.Models
{
    public class SessionModel
    {
        public Session Session { get; set; }
        public List<LogData> Logs { get; set; }
        public List<Observation> ObservationsList { get; set; }
        public List<ProcessWindow> Programms { get; set; }
    }
}