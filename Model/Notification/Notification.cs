using System;
using System.Collections.Generic;

namespace SCMR_Api.Model
{
    public class Notification
    {
        public Notification() { }


        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortContent { get; set; }

        public string Content { get; set; }

        public string ButtonTitle { get; set; }

        public DateTime SendDate { get; set; }

        public DateTime CreateDate { get; set; }

        public bool IsShow { get; set; }

        public NotiifcationType NotiifcationType { get; set; }

        public NotificationShowType ShowType { get; set; }
        

        public virtual IList<NotificationSeen> NotificationSeens { get; set; }


        public string sendDateString
        {
            get
            {
                if (SendDate < new DateTime(0622, 12, 30))
                {
                    return "-----";
                }

                return SendDate.ToPersianDateWithTime();
            }
        }

        public string createDateString
        {
            get
            {
                if (CreateDate < new DateTime(0622, 12, 30))
                {
                    return "-----";
                }

                return CreateDate.ToPersianDateWithTime();
            }
        }

    }

    public enum NotificationShowType
    {
        Student = 0,
        StudentParent = 1,
        Both = 2, // both above
        TMALogin = 3,
        PMALogin = 4,
        DashboardLogin = 5,
        AllLogin = 6
    }

    public enum NotiifcationType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Danger = 3
    }
}