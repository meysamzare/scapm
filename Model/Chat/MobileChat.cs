using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class MobileChat
    {
        public MobileChat() { }

        public int Id { get; set; }

        public MobileChatType SenderType { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }

        public MobileChatType ReciverType { get; set; }
        public int ReciverId { get; set; }
        public string ReciverName { get; set; }


        public string Text { get; set; }

        public bool IsRead { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime? ReciveDate { get; set; }


        public string FileUrl { get; set; }
        [NotMapped]
        public string FileName { get; set; }
        [NotMapped]
        public string FileData { get; set; }



        public string sendDateString
        {
            get
            {
                if (SendDate < new DateTime(0622, 12, 30))
                {
                    return "";
                }

                return SendDate.ToPersianDateWithTime();
            }
        }

        public string reciveDateString
        {
            get
            {
                if (ReciveDate.HasValue)
                {

                    if (ReciveDate.Value < new DateTime(0622, 12, 30))
                    {
                        return "";
                    }

                    return ReciveDate.Value.ToPersianDateWithTime();
                }

                return "";
            }
        }

        public bool detectIsSender(MobileChatType type, int id)
        {
            if (SenderType == type && SenderId == id)
            {
                return true;
            }

            return false;
        }
    }

    public class MobileChatConversation
    {
        public int clientId { get; set; }

        public int clientType { get; set; }

        public string clientImg { get; set; }

        public string clientName { get; set; }

        public string lastChatText { get; set; }

        public int unreadCount { get; set; }

        public DateTime LastChatDateTime { get; set; }

        public string lastChatDate
        {
            get
            {
                return LastChatDateTime.ToPersianDateWithTime();
            }
        }
    }

    public enum MobileChatType
    {
        StudentParent = 0,
        Teacher = 1
    }
}