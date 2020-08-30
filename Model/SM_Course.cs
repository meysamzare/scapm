using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.Course")]
    public class Course
    {
        public Course()
        {

        }


        [Key]
        [Column("Crs.Code")]
        public int Id { get; set; }

        [Column("Crs.Name")]
        public string Name { get; set; }

        [Column("Crs.GradeCode")]
        public int GradeId { get; set; }

        [Column("Crs.CourseMix")]
        public int CourseMix { get; set; }

        [Column("Crs.CourseOrder")]
        public int Order { get; set; }

        [Column("Crs.CourseOrder2")]
        public int Order2 { get; set; }

        [Column("Crs.TeacherCode")]
        public int TeacherId { get; set; }

        [Column("Crs.TeachTime")]
        public int TeachTime { get; set; }


        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }


        public virtual IList<TimeSchedule> TimeSchedules { get; set; }

        public virtual IList<Exam> Exams { get; set; }

        public virtual IList<Question> Questions { get; set; }

        public virtual IList<ClassBook> ClassBooks { get; set; }



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



        public CourseAvgReturn getCourseAverageForStudent(List<ExamScoreForCourseAvg> examScores, Course course)
        {
            var result = new CourseAvgReturn
            {
                CourseId = course.Id,
                CourseName = course.Name
            };

            var count = 0;
            var sumScore = 0.0;

            examScores.ForEach(examScore =>
            {
                count += course.CourseMix;
                sumScore += getJustifiedScore(examScore.Score, examScore.TopScore) * course.CourseMix;
            });

            if (count != 0)
            {
                result.Average = (sumScore / count);
                return result;
            }

            result.Average = 0.0;
            return result;
        }


        private double getJustifiedScore(double score, double topScore)
        {
            double mix = (double)20 / topScore;

            return (score * mix);
        }


    }

    public class CourseAvgReturn
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public double Average { get; set; }
    }

    public class ExamScoreForCourseAvg
    {
        public double Score { get; set; }

        public double TopScore { get; set; }

        public int CourseId { get; set; }
    }
}