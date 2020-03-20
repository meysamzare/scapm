using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.ExamTyp")]
    public class ExamType
    {
        public ExamType()
        {

        }

        [Key]
        [Column("Extyp.id")]
        public int Id { get; set; }


        [Column("Extyp.Name")]
        public string Name { get; set; }

        [Column("Extyp.Desc")]
        public string Desc { get; set; }



        public virtual IList<Exam> Exams { get; set; }


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
    }
}