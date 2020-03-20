using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.StdClassMng")]
    public class StdClassMng
    {
        public StdClassMng() { }

        [Key]
        [Column("StdClassMng.Id")]
        public int Id { get; set; }

        [Column("Std.Code")]
        public int StudentId { get; set; }

        [Column("Std.Classid")]
        public int ClassId { get; set; }

        [Column("Std.Gradeid")]
        public int GradeId { get; set; }

        [Column("Std.YearId")]
        public int YeareducationId { get; set; }

        [Column("Std.insid")]
        public int InsTituteId { get; set; }


        public int? StudentTypeId { get; set; }


        [Column("Std.StudyState")]
        public StdStudyState StudyState { get; set; }

        [Column("Std.BehaveState")]
        public StdBehaveState BehaveState { get; set; }

        [Column("Std.PayrollState")]
        public StdPayrollState PayrollState { get; set; }


        public bool IsActive { get; set; }


        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }
        [ForeignKey("YeareducationId")]
        public virtual Yeareducation Yeareducation { get; set; }
        [ForeignKey("InsTituteId")]
        public virtual InsTitute InsTitute { get; set; }
        
        [ForeignKey("StudentTypeId")]
        public virtual StudentType StudentType { get; set; }



        public virtual IList<StudentScore> StudentScores { get; set; }


        public bool canRemove
        {
            get
            {
                if (Student == null || Student.ExamScores == null)
                {
                    return true;
                }

                if (Student.ExamScores.Any(c => c.Exam.YeareducationId == YeareducationId &&
                    c.Exam.GradeId == GradeId && c.Exam.ClassId == ClassId))
                {
                    return false;
                }

                return true;
            }
        }

        public string studentName
        {
            get
            {
                if (Student == null)
                {
                    return "";
                }

                return Student.Name + " " + Student.LastName;
            }
        }


        public string className
        {
            get
            {
                if (Class == null)
                {
                    return "";
                }

                return Class.Name;
            }
        }


        public string gradeName
        {
            get
            {
                if (Grade == null)
                {
                    return "";
                }

                return Grade.Name;
            }
        }


        public string yeareducationName
        {
            get
            {
                if (Yeareducation == null)
                {
                    return "";
                }

                return Yeareducation.Name;
            }
        }


        public string tituteName
        {
            get
            {
                if (InsTitute == null)
                {
                    return "";
                }

                return InsTitute.Name;
            }
        }


        public string studentTypeName
        {
            get
            {
                if (StudentType == null)
                {
                    return "تعیین نشده";
                }

                return StudentType.Name;
            }
        }


    }

    public enum StdPayrollState
    {
        Comformd = 1,
        WaitingForComform = 2,
        Active = 3,
        DeActive = 4
    }

    public enum StdBehaveState
    {
        Accept = 1,
        WaitingForChecking = 2,
        Conditional = 3
    }

    public enum StdStudyState
    {
        Comformd = 1,
        WaitingForComform = 2,
        Accept = 3,
        Conditional = 4,
        Fired = 5,
        MissFile = 6
    }

}