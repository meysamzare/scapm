using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.Ticket")]
    public class Ticket
    {
        public Ticket() { }

        public int Id { get; set; }

        public string Subject { get; set; }

        public TicketOrder Order { get; set; }

        public TicketState State { get; set; }


        public TicketType SenderType { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }

        public TicketType ReciverType { get; set; }
        public int ReciverId { get; set; }
        public string ReciverName { get; set; }

        public bool IsRemoved { get; set; }


        public virtual IList<TicketConversation> TicketConversations { get; set; }

        public string lastConversationDatePersian
        {
            get
            {
                if (TicketConversations != null && TicketConversations.Any())
                {
                    return TicketConversations.OrderByDescending(c => c.Date).First().Date.ToPersianDateWithTime();
                }

                return "---";
            }
        }

        public int haveNewConversation(Ticket ticket, int userId, TicketType type, IList<TicketConversation> conversations)
        {
            var isSender = false;
            if (ticket.SenderId == userId && ticket.SenderType == type)
            {
                isSender = true;
            }

            var haveNewConversation = new List<bool>();

            foreach (var conver in conversations)
            {
                if (conver.IsSender != isSender && conver.IsSeen == false)
                {
                    haveNewConversation.Add(true);
                }
            }

            return haveNewConversation.Count;
        }

        public bool isSender(Ticket ticket, int userId, TicketType type)
        {
            var isSender = false;

            if (ticket.SenderId == userId && ticket.SenderType == type)
            {
                isSender = true;
            }

            return isSender;
        }

        public bool isImport(Ticket ticket)
        {
            if (ticket.Order == TicketOrder.High)
            {
                return true;
            }

            return false;
        }
    }


    public enum TicketOrder
    {
        Low = 0,
        Modrate = 1,
        High = 2,
        Emergency = 3,
        Request = 4
    }

    public enum TicketState
    {
        Open = 0,
        Close = 1,
        WaitingForAnswer = 2
    }

    public enum TicketType
    {
        Student = 0,
        StudentParent = 1,
        User = 2,
        Teacher = 3
    }
}