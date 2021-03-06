using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.Questions")]
    public class Question
    {
        public Question()
        {

        }


        [Key]
        [Column("Que.id")]
        public int Id { get; set; }


        [Column("Que.Name")]
        public string Name { get; set; }

        [Column("Que.Typ")]
        public QueType Type { get; set; }

        [Column("Que.Question")]
        public string Title { get; set; }



        [Column("Que.Courseid")]
        public int CourseId { get; set; }

        [Column("Que.Gradeid")]
        public int GradeId { get; set; }

        [Column("Que.SourceCreation")]
        public string Person { get; set; }


        [Column("Que.Source")]
        public string Source { get; set; }

        [Column("Que.Mark")]
        public double Mark { get; set; }

        [Column("Que.Defact")]
        public QueDefact Defact { get; set; }

        [Column("Que.Answer")]
        public string Answer { get; set; }

        [Column("Que.Description")]
        public string Desc1 { get; set; }

        [Column("Que.Desc")]
        public string Desc2 { get; set; }





        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }


        public virtual IList<QuestionOption> QuestionOptions { get; set; }


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

        public string courseName
        {
            get
            {
                if (Course == null)
                {
                    return "";
                }

                return Course.Name;
            }
        }

        public string markString
        {
            get
            {
                return Mark.ToString("#.##");
            }
        }


    }


    public enum QueType
    {
        Tashrihe = 1,
        Test = 2,
        Replace = 3,
        True_False = 4
    }

    public enum QueDefact
    {
        Hard = 1,
        Modrate = 2,
        Easy = 3
    }
}