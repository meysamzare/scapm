using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.QuestionOptions")]
    public class QuestionOption
    {
        public QuestionOption() { }

        [Key]
        [Column("QueOp.id")]
        public int Id { get; set; }

        public string Title { get; set; }

        [Column("QueOp.name")]
        public string Name { get; set; }

        [Column("QueOp.isAnswer")]
        public bool IsTrue { get; set; }


        [Column("QueOp.Questionid")]
        public int QuestionId { get; set; }

        [NotMapped]
        public virtual List<OnlineExamAnswer> OnlineExamAnswers { get; set; }


        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }


        public string questionName
        {
            get
            {
                if (Question == null)
                {
                    return "";
                }
                return Question.Name;
            }
        }

    }
}