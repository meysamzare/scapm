using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace SCMR_Api.Model
{
    [Table("sm.Yeareducation")]
    public class Yeareducation
    {
        public Yeareducation()
        {

        }

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




        public virtual IList<Grade> Grades { get; set; }

        public virtual IList<StdClassMng> StdClassMngs { get; set; }

        public virtual IList<Exam> Exams { get; set; }


        public virtual IList<ClassBook> ClassBooks { get; set; }


        public bool haveGrade
        {
            get
            {
                if (Grades == null)
                {
                    return false;
                }
                return Grades.Any();
            }
        }

        public bool haveExam
        {
            get
            {
                if (Exams == null)
                {
                    return false;
                }
                return Exams.Any();
            }
        }



        public string DateStartPersian
        {
            get
            {
                return DateStart.ToPersianDate();
            }
        }

        public string DateEndPersian
        {
            get
            {
                return DateEnd.ToPersianDate();
            }
        }

    }
}