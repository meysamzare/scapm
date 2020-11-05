using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class OnlineExamAnswer
    {
        public OnlineExamAnswer() { }

        [Key]
        public long Id { get; set; }

        public int StudentId { get; set; }

        public long OnlineExamId { get; set; }

        public int QuestionOptionId { get; set; }



        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        [ForeignKey("OnlineExamId")]
        public OnlineExam OnlineExam { get; set; }

        [ForeignKey("QuestionOptionId")]
        public QuestionOption QuestionOption { get; set; }
    }
}