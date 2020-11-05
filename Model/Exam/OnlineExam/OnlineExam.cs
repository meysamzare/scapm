using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class OnlineExam
    {
        public OnlineExam() { }

        [Key]
        public long Id { get; set; }

        public string Title { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public int GradeId { get; set; }
        public int? ClassId { get; set; }

        public bool RandomQuestion { get; set; }
        public bool RandomQuestionOption { get; set; }


        public virtual List<OnlineExamQuestion> OnlineExamQuestions { get; set; }

        public virtual List<OnlineExamAnswer> OnlineExamAnswers { get; set; }



        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
    }
}