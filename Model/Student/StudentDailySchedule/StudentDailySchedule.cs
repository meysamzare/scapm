using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SCMR_Api.Model
{
    public class StudentDailySchedule
    {
        public StudentDailySchedule() { }


        public int Id { get; set; }

        public int StdClassMngId { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime DateExecute { get; set; }

        public int CourseId { get; set; }

        public StudentDailyScheduleState State { get; set; }

        public StudentDailyScheduleType Type { get; set; }

        public string Volume { get; set; }

        public string FromTime { get; set; }
        public string ToTime { get; set; }


        public string StudentParentComment { get; set; }
        public DateTime? StudentParentCommentDate { get; set; }

        public string ConsultantComment { get; set; }
        public DateTime? ConsultantCommentDate { get; set; }


        [ForeignKey("StdClassMngId")]
        public virtual StdClassMng StdClassMng { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }



        public string dateCreateString => DateCreate.ToPersianDate();
        public string dateExecuteString => DateExecute.ToPersianDate();
        public string dateExecuteDayString => DateExecute.PersianDayString();

        public string studentParentCommentDateString => StudentParentCommentDate.ToPersianDate();
        public string consultantCommentDateString => ConsultantCommentDate.ToPersianDate();

        public int studentId => StdClassMng != null ? StdClassMng.StudentId : 0;

        public string courseName => Course != null ? Course.Name : "";

        public string getTypeString(StudentDailyScheduleType type)
        {
            if (type == StudentDailyScheduleType.Motalee)
            {
                return "مطالعه";
            }
            if (type == StudentDailyScheduleType.TestMotalee)
            {
                return "تست مطالعه";
            }
            if (type == StudentDailyScheduleType.TestMoroor)
            {
                return "تست مرور";
            }
            if (type == StudentDailyScheduleType.Moroor)
            {
                return "مرور";
            }
            if (type == StudentDailyScheduleType.Jambandi)
            {
                return "جمع بندی";
            }
            if (type == StudentDailyScheduleType.HalTamrin)
            {
                return "حل تمرین";
            }

            return "";
        }

        public string typeString => getTypeString(Type);

    }
}

public enum StudentDailyScheduleState
{
    NotLooked = 1,
    Complated,
    Empty,
    InComplate
}

public enum StudentDailyScheduleType
{
    Motalee = 1,
    TestMotalee,
    TestMoroor,
    Moroor,
    Jambandi,
    HalTamrin
}