using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class NotificationSeen
    {
        public NotificationSeen()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int NotificationId { get; set; }

        public string AgentName { get; set; }

        public int AgentId { get; set; }

        // 0 for Student and 1 for StudentParent
        public int AgentType { get; set; }


        public DateTime Date { get; set; }
        public int GradeId { get; set; }
        public int ClassId { get; set; }


        public string getAgentFullName(int agentType, string agentName)
        {
            string type = agentType == 0 ? "دانش آموز " : "اولیای ";

            return type + agentName;
        }

        public string agentFullName
        {
            get
            {
                return getAgentFullName(AgentType, AgentName);
            }
        }

        public string dateString => Date.ToPersianDateWithTime();

    }
}