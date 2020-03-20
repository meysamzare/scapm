using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{

    public class StudentScore
    {
        public StudentScore() { }

        public int Id { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public int Value { get; set; }

        public DateTime DateCreate { get; set; }

        public int TeacherId { get; set; }

        public int StdClassMngId { get; set; }

        [ForeignKey("StdClassMngId")]
        public virtual StdClassMng StdClassMng { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        public string studentFullName
        {
            get
            {
                if (StdClassMng == null)
                {
                    return "";
                }

                return StdClassMng.studentName;
            }
        }

        public int studentId
        {
            get
            {
                if (StdClassMng == null)
                {
                    return 0;
                }

                return StdClassMng.StudentId;
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

        public string dateCreateString
        {
            get
            {
                if (DateCreate < new DateTime(0622, 12, 30))
				{
					return "";
				}

                return DateCreate.ToPersianDate();
            }
        }
    }
}