using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Teacher
    {
        public Teacher()
        {

        }

        [Key]
        [Column("Tch.Autonum")]
        public int Id { get; set; }

        [Column("Tch.Id")]
        public int Code { get; set; }

        [Column("Tch.Name")]
        public string Name { get; set; }

        [Column("Tch.PrsCode")]
        public int OrgPersonId { get; set; }



        [Column("Tch.Password")]
        public string Password { get; set; }

        public bool AllCourseAccess { get; set; }



        [ForeignKey("OrgPersonId")]
        public virtual OrgPerson OrgPerson { get; set; }

        public virtual IList<TimeSchedule> TimeSchedules { get; set; }

        public virtual IList<Course> Courses { get; set; }

        public virtual IList<Exam> Exams { get; set; }

        public virtual IList<ClassBook> ClassBooks { get; set; }

        public virtual IList<StudentScore> StudentScores { get; set; }



        public bool haveTimeSchedules
        {
            get
            {
                if (TimeSchedules == null)
                {
                    return false;
                }

                return TimeSchedules.Any();
            }
        }

        public bool haveCourses
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

        public string getPersonelCode
        {
            get
            {
                if (OrgPerson == null)
                {
                    return "";
                }
                return OrgPerson.Code.ToString();
            }
        }

    }
}