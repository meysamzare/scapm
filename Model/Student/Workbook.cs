using System.Collections.Generic;

namespace SCMR_Api.Model
{

    public class Workbook
    {
        public Workbook() { }

        public int Id { get; set; }
        

        public string Name { get; set; }

        public int Code { get; set; }

        public bool IsShow { get; set; }



        public virtual IList<Exam> Exams { get; set; }
    }
}