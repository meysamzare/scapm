using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class OnlineExamQuestion
    {
        public OnlineExamQuestion() { }

        [Key]
        public long Id { get; set; }

        public long OnlineExamId { get; set; }
        public int QuestionId { get; set; }

        [ForeignKey("OnlineExamId")]
        public virtual OnlineExam OnlineExam { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
    }
}