using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model
{
    [Table("Tbl_WorkBook_Details")]
    public class WorkBookDetail
    {
        public WorkBookDetail()
        {
            
        }

        public int Id { get; set; }

        public int StdId { get; set; }

        public string StdName { get; set; }

        public int ColRow { get; set; }

        public string ColCourseName { get; set; }

        public string ColExam1 { get; set; }

        public string ColExam2 { get; set; }

        public string ColExam3 { get; set; }

        public string ColExam4 { get; set; }

        public string ColExam12 { get; set; }

        public string ColExam34 { get; set; }

        public string ColSumExam { get; set; }

        public string ColYearExam { get; set; }

        public string ColMinInClass { get; set; }

        public string ColMaxInClass { get; set; }

        public string ColAvgExam { get; set; }

        public string ColExamPercent { get; set; }

        public string ColScoreInClass { get; set; }

        public string ColScoreInGrade { get; set; }

        public bool haveRequestToReview { get; set; }

    }
}