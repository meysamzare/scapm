using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.Grade")]
    public class Grade
    {
        public Grade() { }

        [Key]
        [Column("Grd.Autonumber")]
        public int Id { get; set; }

        [Column("Grd.Id")]
        public string Code { get; set; }

        [Column("Grd.Name")]
        public string Name { get; set; }

        [Column("Grd.insCode")]
        public int TituteId { get; set; }

        [Column("Grd.eduyearCode")]
        public int YeareducationId { get; set; }

        [Column("Grd.Capasity")]
        public int Capasity { get; set; }

        [Column("Grd.OrgCode")]
        public string OrgCode { get; set; }

        [Column("Grd.InternalCode")]
        public string InternalCode { get; set; }

        [Column("Grd.Desc")]
        public string Desc { get; set; }

        [Column("Grd.Order")]
        public int Order { get; set; }



        [ForeignKey("TituteId")]
        public virtual InsTitute Titute { get; set; }

        [ForeignKey("YeareducationId")]
        public virtual Yeareducation Yeareducation { get; set; }

        public virtual IList<Class> Classes { get; set; }
        public virtual IList<Course> Courses { get; set; }

        public virtual IList<StdClassMng> StdClassMngs { get; set; }

        public virtual IList<Exam> Exams { get; set; }
        public virtual IList<Question> Questions { get; set; }

        public virtual IList<ClassBook> ClassBooks { get; set; }


        [NotMapped]
        public virtual IList<OnlineExam> OnlineExams { get; set; }
        

        public bool haveClass
        {
            get
            {
                if (Classes == null)
                {
                    return false;
                }

                return Classes.Any();
            }
        }

        //TODO: Add to grade list
        public bool haveCourse
        {
            get
            {
                if (Courses == null)
                {
                    return false;
                }

                return Courses.Any();
            }
        }

        public string tituteName
        {
            get
            {
                if (Titute == null)
                {
                    return "";
                }
                return Titute.Name;
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
    }
}