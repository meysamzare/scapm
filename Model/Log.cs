using System;

namespace SCMR_Api.Model
{
    public class Log
    {
        public Log() { }

        public int Id { get; set; }

        public string Event { get; set; }

        public string Desc { get; set; }

        public string Ip { get; set; }

        public DateTime Date { get; set; }

        public int agentId { get; set; }

        public string agnetType { get; set; }

        public string agentName { get; set; }

        public string LogSource { get; set; }

        public string dateString
        {
            get
            {
                return Date.ToPersianDate();
            }
        }
    }
}
