using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.Class")]
    public class Class
    {
        public Class()
        {

        }


        [Key]
        [Column("Cls.Autonumber")]
        public int Id { get; set; }

        [Column("Cls.Id")]
        public int Code { get; set; }

        [Column("Cls.Name")]
        public string Name { get; set; }

        [Column("Cls.Section")]
        public string Section { get; set; }

        [Column("Grd.Id")]
        public int GradeId { get; set; }

        [Column("Cls.Capasity")]
        public int Capasity { get; set; }

        [Column("Cls.Order")]
        public int Order { get; set; }


        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }


        public virtual IList<StdClassMng> StdClassMngs { get; set; }

        public virtual IList<Exam> Exams { get; set; }

        public virtual IList<ClassBook> ClassBooks { get; set; }

        [NotMapped]
        public virtual IList<OnlineExam> OnlineExams { get; set; }


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

        public bool haveStdClassMngs
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
    }
}