using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class OnlineClassLogin
    {
        public OnlineClassLogin() { }

        public long Id { get; set; }

        public long OnlineClassId { get; set; }

        public DateTime Date { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string IP { get; set; }

        public int UserId { get; set; }

        public OnlineClassAgentType AgentType { get; set; }




        [ForeignKey("OnlineClassId")]
        public virtual OnlineClass OnlineClass { get; set; }
    }

    public enum OnlineClassAgentType
    {
        Dashboard = 0,
        TMA = 1,
        PMA = 2
    }
}