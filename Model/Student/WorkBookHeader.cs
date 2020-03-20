using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model
{
    [Table("Tbl_WorkBook_Header")]
    public class WorkBookHeader
    {
        public WorkBookHeader()
        {
            
        }

        public int Id { get; set; }

        [Key]
        public int StdId { get; set; }

        public string StdName { get; set; }

        public string Class { get; set; }

        public string Year { get; set; }

        public string enzebat1 { get; set; }
        public string enzebat2 { get; set; }

        public string jame1 { get; set; }

        public string jame2 { get; set; }
        public string jamekol { get; set; }

        public string nobat1 { get; set; }

        public string nobat2 { get; set; }

        public string moadelkol { get; set; }

        public string rotbeclass { get; set; }

        public string rotbepayeh { get; set; }



        
        public int state { get; set; }

    }
}