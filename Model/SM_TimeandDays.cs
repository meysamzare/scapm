using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class TimeandDays
    {
        public TimeandDays()
        {

        }

        [Key]
        [Column("Td.Autonum")]
        public int Id { get; set; }


        [Column("Td.Name")]
        public string Name { get; set; }



        [Column("Td.sat")]
        public bool sat { get; set; }

        [Column("Td.sun")]
        public bool sun { get; set; }

        [Column("Td.mon")]
        public bool mon { get; set; }

        [Column("Td.tue")]
        public bool tue { get; set; }

        [Column("Td.wed")]
        public bool wed { get; set; }

        [Column("Td.thr")]
        public bool thr { get; set; }

        [Column("Td.fri")]
        public bool fri { get; set; }



        public virtual IList<TimeSchedule> TimeSchedules { get; set; }

        public bool haveTimeSchedules
        {
            get
            {
                if (TimeSchedules == null)
                {
                    return false;
                }

                return TimeSchedules.Any();
            }
        }

        public string[] getNameOfSelectedDays
        {
            get
            {
                var s = new List<string>();

                if (sat)
                {
                    s.Add("شنبه");
                }
                if (sun)
                {
                    s.Add("یکشنبه");
                }
                if (mon)
                {
                    s.Add("دوشنبه");
                }
                if (tue)
                {
                    s.Add("سه شنبه");
                }
                if (wed)
                {
                    s.Add("چهار شنبه");
                }
                if (thr)
                {
                    s.Add("پنج شنبه");
                }
                if (fri)
                {
                    s.Add("جمعه");
                }


                if (!s.Any())
                {
                    s.Add("هیچ روزی انتخاب نشده است");
                }

                return s.ToArray();
            }
        }
    }
}