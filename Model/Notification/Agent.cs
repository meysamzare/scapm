using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebPush;

namespace SCMR_Api.Model
{
    public class NotificationAgent
    {
        public NotificationAgent() { }

        public int Id { get; set; }

        public DateTime SubscribeDate { get; set; }

        public int StudentId { get; set; }

        public bool IsParent { get; set; }


        public string Endpoint { get; set; }
        public string P256DH { get; set; }
        public string Auth { get; set; }


        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }


        public string studentFullNameAndType
        {
            get
            {
                if (Student != null)
                {
                    return (IsParent ? "اولیای" : "") + Student.Name + " " + Student.LastName;
                }

                return "";
            }
        }


        public PushSubscription getPushSubscription
        {
            get
            {
                return new PushSubscription
                {
                    Endpoint = Endpoint,
                    Auth = Auth,
                    P256DH = P256DH
                };
            }
        }


        public string subscribeDateString
        {
            get
            {
                if (SubscribeDate < new DateTime(0622, 12, 30))
                {
                    return "";
                }
                return SubscribeDate.ToPersianDateWithTime();
            }
        }

        
        public bool isStudentContainsGradeId(int? gradeId, IList<StdClassMng> stdClassMngs)
        {
            bool isContains = false;

            if (gradeId.HasValue)
            {
                foreach (var item in stdClassMngs)
                {
                    if (item.GradeId == gradeId)
                    {
                        isContains = true;

                        break;
                    }
                }
            }

            return isContains;
        }

        public bool isStudentContainsClassId(int? classId, IList<StdClassMng> stdClassMngs)
        {
            bool isContains = false;

            if (classId.HasValue)
            {
                foreach (var item in stdClassMngs)
                {
                    if (item.ClassId == classId)
                    {
                        isContains = true;

                        break;
                    }
                }
            }

            return isContains;
        }


    }
}