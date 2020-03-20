using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SCMR_Api.Model.Financial;

namespace SCMR_Api.Model
{
    [Table("sm.Student")]
    public class Student
    {
        public Student()
        {
            ParentsPassword = "1";
            StudentPassword = "1";
        }

        [Key]
        [Column("Std.Autonum")]
        public int Id { get; set; }

        [Column("Std.Code")]
        public int Code { get; set; }

        [Column("Std.OrgCode")]
        public string OrgCode { get; set; }

        [Column("Std.Name")]
        public string Name { get; set; }

        [Column("Std.LastName")]
        public string LastName { get; set; }

        public string fullName => Name + " " + LastName;

        [Column("Std.FatherName")]
        public string FatherName { get; set; }

        [Column("Std.IdNumber")]
        public string IdNumber { get; set; }

        [Column("Std.IdNumber2")]
        public string IdNumber2 { get; set; }

        [Column("Std.BirthDate")]
        public DateTime BirthDate { get; set; }

        [Column("Std.IdCardSerial")]
        public string IdCardSerial { get; set; }

        [Column("Std.BirthLocation")]
        public string BirthLocation { get; set; }


        [Column("Std.State")]
        public int StudentState { get; set; }

        [Column("Std.PictureUrl")]
        public string PicUrl { get; set; }

        [NotMapped]
        public string PicData { get; set; }
        [NotMapped]
        public string PicName { get; set; }


        //TODO: We May wanna to add a username fild later.....
        public string ParentsPassword { get; set; }
        public string StudentPassword { get; set; }

        public bool ParentAccess { get; set; }
        public bool StudentAccess { get; set; }



        public virtual IList<StdClassMng> StdClassMngs { get; set; }

        public virtual StudentInfo StudentInfo { get; set; }

        public virtual IList<ExamScore> ExamScores { get; set; }


        public virtual IList<StdPayment> StudentPayments { get; set; }

        public virtual IList<NotificationAgent> NotificationAgents { get; set; }

        public virtual IList<ClassBook> ClassBooks { get; set; }


        public bool isStudentInfoComplated
        {
            get
            {
                if (StudentInfo != null)
                {
                    if (StudentInfo.isAllHaveValue)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }


        // TODO: Add To Client
        public bool haveStdPayment
        {
            get
            {
                if (StudentPayments == null)
                {
                    return false;
                }
                return StudentPayments.Any();
            }
        }

        public bool haveStdClassMng
        {
            get
            {
                if (StdClassMngs == null)
                {
                    return false;
                }

                return StdClassMngs.Any();
            }
        }

        public int stdClassMngNumber
        {
            get
            {
                if (StdClassMngs == null)
                {
                    return 0;
                }
                return StdClassMngs.Count;
            }
        }

        public string birthDateString
        {
            get
            {
                return BirthDate.ToPersianDate();
            }
        }



        public bool haveState(int type, List<StdClassMng> stdClassMngs, int state)
        {
            var activeStdClassMng = stdClassMngs.Where(c => c.IsActive == true).FirstOrDefault();

            if (activeStdClassMng == null)
            {
                return false;
            }

            if (type == 1)
            {
                return activeStdClassMng.StudyState == (StdStudyState)state;
            }

            if (type == 2)
            {
                return activeStdClassMng.BehaveState == (StdBehaveState)state;
            }

            if (type == 3)
            {
                return activeStdClassMng.PayrollState == (StdPayrollState)state;
            }

            return false;            
        }

        public bool haveGrade(int gradeId, List<StdClassMng> stdClassMngs)
        {
            var activeStdClassMng = stdClassMngs.Where(c => c.IsActive == true).FirstOrDefault();

            
            if (activeStdClassMng == null)
            {
                return false;
            }

            return activeStdClassMng.GradeId == gradeId;
        }

        public bool haveClass(int classId, List<StdClassMng> stdClassMngs)
        {
            var activeStdClassMng = stdClassMngs.Where(c => c.IsActive == true).FirstOrDefault();

            
            if (activeStdClassMng == null)
            {
                return false;
            }

            return activeStdClassMng.ClassId == classId;
        }
    }

    public enum StudentState
    {
        PreSubmit = 1,
        Submit = 2,
        Comformd = 3,
        WaitingForComform = 4,
        NotComformd = 5,
        MissFile = 6
    }
}