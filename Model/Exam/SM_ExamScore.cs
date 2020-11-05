using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class ExamScore
    {
        public ExamScore() { }


        [Key]
        [Column("Exsc.id")]
        public int Id { get; set; }



        [Column("Exsc.Examid")]
        public int ExamId { get; set; }

        [Column("Exsc.Studentid")]
        public int StudentId { get; set; }



        [Column("Ex.Score")]
        public double Score { get; set; }



        public int? DescriptiveScoreId { get; set; }



        [Column("Ex.TopScore")]
        public int TopScore { get; set; }

        [Column("Ex.AmauntQuestion")]
        public int NumberQ { get; set; }



        [Column("Ex.CorrectAnswer")]
        public int TrueAnswer { get; set; }

        [Column("Ex.WrongAnswer")]
        public int FalseAnswer { get; set; }

        [Column("Ex.BlankAnswer")]
        public int BlankAnswer { get; set; }


        public ExamScoreState State { get; set; }


        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }


        [ForeignKey("DescriptiveScoreId")]
        public virtual DescriptiveScore DescriptiveScore { get; set; }

        public string getDescriptiveName(DescriptiveScore descriptiveScore)
        {
            if (descriptiveScore != null)
            {
                return descriptiveScore.Name;
            }

            return "";
        }


        public string descriptiveName => getDescriptiveName(DescriptiveScore);


        public string getRating(List<ExamScore> examScores, double score)
        {
            if (examScores != null)
            {
                if (examScores.Any(c => c.State == ExamScoreState.Hazer))
                {
                    var distList = examScores
                        .Where(c => c.State == ExamScoreState.Hazer).OrderByDescending(c => c.Score)
                            .Select(c => c.Score)
                        .Distinct()
                    .ToList();

                    var rate = "";

                    try
                    {
                        rate = (distList.FindIndex(c => c == score) + 1).ToString();
                    }
                    catch
                    {
                        rate = "---";
                    }

                    return rate;
                }
            }

            return "";
        }

        public string Rating
        {
            get
            {
                if (Exam != null && Exam.ExamScores != null)
                {
                    var examScores = new List<ExamScore>(Exam.ExamScores);

                    if (examScores.Any(c => c.State == ExamScoreState.Hazer))
                    {
                        var distList = examScores
                            .Where(c => c.State == ExamScoreState.Hazer).OrderByDescending(c => c.Score)
                                .Select(c => c.Score)
                            .Distinct()
                        .ToList();

                        var rate = "";

                        try
                        {
                            rate = (distList.FindIndex(c => c == Score) + 1).ToString();
                        }
                        catch
                        {
                            rate = "---";
                        }

                        return rate;
                    }
                }

                return "";
            }
        }

        public string studentName
        {
            get
            {
                if (Student == null)
                {
                    return "";
                }

                return Student.Name + " " + Student.LastName;
            }
        }

        public string examName
        {
            get
            {
                if (Exam == null)
                {
                    return "";
                }

                return Exam.Name;
            }
        }

        public string scoreString
        {
            get
            {
                if (State == ExamScoreState.Hazer)
                {
                    return Score.ToString("0.##");
                }

                if (State == ExamScoreState.TrueGhaeb)
                {
                    return "غائب و موجه";
                }

                if (State == ExamScoreState.FalseGhaeb)
                {
                    return "غائب و غیرموجه";
                }

                return Score.ToString("0.##");
            }
        }
    }

    public enum ExamScoreState
    {
        Hazer = 0,
        TrueGhaeb = 1,
        FalseGhaeb = 2
    }
}