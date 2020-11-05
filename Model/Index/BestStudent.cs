using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model.Index
{
    public class BestStudent
    {
        public BestStudent() { }


        public int Id { get; set; }

        public string FullName { get; set; }

        public string Title { get; set; }

        public string Class { get; set; }

        public string Desc { get; set; }


        public string PicUrl { get; set; }
        [NotMapped]
        public string PicData { get; set; }
        [NotMapped]
        public string PicName { get; set; }


        public DateTime DateStart { get; set; }

        public DateTime DateEnd { get; set; }



        public string dateStartString
        {
            get
            {
                return DateStart.ToPersianDate();
            }
        }

        public string dateEndString
        {
            get
            {
                return DateEnd.ToPersianDate();
            }
        }

    }
}