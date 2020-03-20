using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    [Table("sm.TimeSchedule")]
    public class TimeSchedule
    {
        public TimeSchedule()
        {

        }

        [Key]
        [Column("Tsch.Autonum")]
        public int Id { get; set; }

        [Column("Tsch.Name")]
        public string Name { get; set; }

        [Column("Tsch.CourseCode")]
        public int CourseId { get; set; }

        [Column("Tsch.TeacherCode")]
        public int TeacherId { get; set; }

        [Column("Tsch.TimeStart")]
        public TimeSpan TimeStart { get; set; }

        [Column("Tsch.TimeEnd")]
        public TimeSpan TimeEnd { get; set; }

        [Column("Tsch.DaysId")]
        public int TimeandDaysId { get; set; }



        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("TimeandDaysId")]
        public virtual TimeandDays TimeandDays { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }



        public string courseTitle
        {
            get
            {
                if (Course == null)
                {
                    return "";
                }
                else
                {
                    return Course.Name;
                }
            }
        }

        public string timeandDaysTitle
        {
            get
            {
                if (TimeandDays == null)
                {
                    return "";
                }
                else
                {
                    return TimeandDays.Name;
                }
            }
        }

        public string teacherTitle
        {
            get
            {
                if (Teacher == null)
                {
                    return "";
                }
                else
                {
                    return Teacher.Name;
                }
            }
        }

    }
}