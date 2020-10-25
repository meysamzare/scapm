using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.Yeareducation")]
    public class Yeareducation
    {
        public Yeareducation() { }

        [Key]
        [Column("edu.YeareduCode")]
        public int Id { get; set; }

        [Column("edu.YeareduName")]
        public string Name { get; set; }

        [Column("edu.DateStart")]
        public DateTime DateStart { get; set; }

        [Column("edu.DateEnd")]
        public DateTime DateEnd { get; set; }

        [Column("edu.Desc")]
        public string Desc { get; set; }

        public bool IsActive { get; set; }


        public YeareducationScoreType ScoreType { get; set; }


        public virtual IList<Grade> Grades { get; set; }
        public virtual IList<StdClassMng> StdClassMngs { get; set; }
        public virtual IList<Exam> Exams { get; set; }
        public virtual IList<ClassBook> ClassBooks { get; set; }


        public bool haveGrade => Grades == null ? false : Grades.Any();
        public bool haveExam => Exams == null ? false : Exams.Any();
        public string DateStartPersian => DateStart.ToPersianDate();
        public string DateEndPersian => DateEnd.ToPersianDate();



    }

    public enum YeareducationScoreType
    {
        Normal = 1,
        Descriptive
    }
}