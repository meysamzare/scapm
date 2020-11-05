using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class QuestionCategory
    {
        public QuestionCategory() { }

        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }

        public virtual List<Question> Questions { get; set; }
    }
}