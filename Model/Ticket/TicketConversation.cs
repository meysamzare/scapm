using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    [Table("sm.TicketConversation")]
    public class TicketConversation
    {
        public TicketConversation() { }

        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }

        public int Rate { get; set; }


        public string FileUrl { get; set; }
        [NotMapped]
        public string FileData { get; set; }
        [NotMapped]
        public string FileName { get; set; }


        public bool IsSender { get; set; }

        public bool IsSeen { get; set; }


        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }



        public string dateString
        {
            get
            {
                if (Date < new DateTime(0622, 12, 30))
                {
                    return "";
                }
                return Date.ToPersianDateWithTime();
            }
        }
    }

}