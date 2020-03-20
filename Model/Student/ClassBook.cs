using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{

    public class ClassBook
    {
        public ClassBook() { }


        public int Id { get; set; }


        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        public int InsTituteId { get; set; }
        [ForeignKey("InsTituteId")]
        public virtual InsTitute InsTitute { get; set; }

        public int YeareducationId { get; set; }
        [ForeignKey("YeareducationId")]
        public virtual Yeareducation Yeareducation { get; set; }

        public int GradeId { get; set; }
        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }



        public DateTime Date { get; set; }

        public ClassBookType Type { get; set; }

        public string Value { get; set; }

        public int? ExamId { get; set; }
        public string ExamName { get; set; }

        public ClassBookRegisterType RegisterType { get; set; }

        public int RegisterId { get; set; }



        public string dateString
        {
            get
            {
                return Date.ToPersianDate();
            }
        }


        public string teacherName
        {
            get
            {
                if (Teacher == null)
                {
                    return "";
                }
                
                return Teacher.Name;
            }
        }

        public string courseName
        {
            get
            {
                if (Course == null)
                {
                    return "";
                }

                return Course.Name;
            }
        }
    }

    public enum ClassBookType
    {
        P_A = 0,
        ExamScore = 1,
        ClassAsk = 2,
        Point = 3,
        Discipline = 4
    }

    public enum ClassBookRegisterType
    {
        User = 0,
        Teacher = 1
    }
}