using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Exam
    {
        public Exam() { }


        [Key]
        [Column("Exm.id")]
        public int Id { get; set; }

        [Column("Ex.name")]
        public string Name { get; set; }

        [Column("Ex.Date")]
        public DateTime Date { get; set; }

        [Column("Ex.AmauntQuestion")]
        public int NumberQ { get; set; }

        [Column("Ex.Source")]
        public string Source { get; set; }

        [Column("Ex.TopScore")]
        public double TopScore { get; set; }

        [Column("Ex.ExamTyp")]
        public int? ExamTypeId { get; set; }

        [Column("Ex.Grade")]
        public int GradeId { get; set; }

        [Column("Ex.Class")]
        public int? ClassId { get; set; }

        [Column("Ex.Teacher")]
        public int? TeacherId { get; set; }

        [Column("Ex.Order")]
        public int Order { get; set; }

        [Column("Ex.Course")]
        public int CourseId { get; set; }
        
        public int? WorkbookId { get; set; }

        [Column("Ex.Time")]
        public int Time { get; set; }

        [Column("Ex.Result")]
        public bool Result { get; set; }

        [Column("Ex.ResultDate")]
        public DateTime? ResultDate { get; set; }

        public bool IsCancelled { get; set; }
        public string CancellReason { get; set; }


        public bool ShowAvgOfExam { get; set; }


        public int? OnlineExamId { get; set; }


        public int? ParentId { get; set; }


        [ForeignKey("ParentId")]
        public virtual Exam Parent { get; set; }
        public virtual IList<Exam> Children { get; set; }


        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("ExamTypeId")]
        public virtual ExamType ExamType { get; set; }

        [ForeignKey("WorkbookId")]
        public virtual Workbook Workbook { get; set; }



        public virtual IList<ExamScore> ExamScores { get; set; }

        public bool canShowByWorkBook(Workbook workbook)
        {
            if (workbook != null)
            {
                return workbook.IsShow;
            }
            
            return true;
        }

        public double getMin_Max_Avg_InExam(IList<ExamScore> ExamScores, ScoreType type)
        {
            if (ExamScores == null)
            {
                return 0;
            }

            if (!ExamScores.Any())
            {
                return 0;
            }

            if (!ExamScores.Where(c => c.State == ExamScoreState.Hazer).Any())
            {
                return 0;
            }

            if (type == ScoreType.Min)
            {
                return double.Parse(ExamScores.Where(c => c.State == ExamScoreState.Hazer).Min(c => c.Score).ToString("0.##"));
            }

            if (type == ScoreType.Avg)
            {
                return double.Parse(ExamScores.Where(c => c.State == ExamScoreState.Hazer).Average(c => c.Score).ToString("0.##"));
            }

            if (type == ScoreType.Max)
            {
                return double.Parse(ExamScores.Where(c => c.State == ExamScoreState.Hazer).Max(c => c.Score).ToString("0.##"));
            }

            return 0;
        }



        public double avgInExam
        {
            get
            {
                if (ExamScores == null)
                {
                    return 0;
                }

                if (!ExamScores.Any())
                {
                    return 0;
                }

                if (!ExamScores.Where(c => c.State == ExamScoreState.Hazer).Any())
                {
                    return 0;
                }

                return double.Parse(ExamScores.Where(c => c.State == ExamScoreState.Hazer).Average(c => c.Score).ToString("0.##"));
            }
        }

        public double minInExam
        {
            get
            {
                if (ExamScores == null)
                {
                    return 0;
                }

                if (!ExamScores.Any())
                {
                    return 0;
                }
                if (!ExamScores.Where(c => c.State == ExamScoreState.Hazer).Any())
                {
                    return 0;
                }

                return double.Parse(ExamScores.Where(c => c.State == ExamScoreState.Hazer).Min(c => c.Score).ToString("#,#0"));
            }
        }

        public double maxInExam
        {
            get
            {
                if (ExamScores == null)
                {
                    return 0;
                }

                if (!ExamScores.Any())
                {
                    return 0;
                }
                if (!ExamScores.Where(c => c.State == ExamScoreState.Hazer).Any())
                {
                    return 0;
                }

                return double.Parse(ExamScores.Where(c => c.State == ExamScoreState.Hazer).Max(c => c.Score).ToString("#,#0"));
            }
        }

        public string resultTime
        {
            get
            {
                if (ResultDate.HasValue)
                {
                    return ResultDate.Value.ToString("HH:mm");
                }

                return "00:00";
            }
        }


        public bool haveChildren
        {
            get
            {
                if (Children == null)
                {
                    return false;
                }
                return Children.Any();
            }
        }

        public bool haveScore
        {
            get
            {
                if (ExamScores == null)
                {
                    return false;
                }
                return ExamScores.Any();
            }
        }


        public string parentName
        {
            get
            {
                if (Parent == null || ParentId == null)
                {
                    return "ریشه";
                }
                else
                {
                    return Parent.Name;
                }
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
                else
                {
                    return Grade.Name;
                }
            }
        }

        public string className
        {
            get
            {
                if (Class == null)
                {
                    return "";
                }
                else
                {
                    return Class.Name;
                }
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
                else
                {
                    return Teacher.Name;
                }
            }
        }

        public string courseName
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

        public string examTypeName
        {
            get
            {
                if (ExamType == null)
                {
                    return "";
                }
                else
                {
                    return ExamType.Name;
                }
            }
        }

        public string dateString
        {
            get
            {
                return Date.ToPersianDate();
            }
        }

    }
}

public enum ScoreType
{
    Min = 1,
    Avg = 2,
    Max = 3
}