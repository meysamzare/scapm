using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.OrgPerson")]
    public class OrgPerson
    {
        public OrgPerson()
        {

        }


        [Key]
        [Column("Orgprs.Autonum")]
        public int Id { get; set; }

        [Column("Orgprs.Code")]
        public int Code { get; set; }

        [Column("Orgprs.Name")]
        public string Name { get; set; }

        [Column("Orgprs.LastName")]
        public string LastName { get; set; }

        [Column("Orgprs.FatherName")]
        public string FatherName { get; set; }

        [Column("Orgprs.sex")]
        public sex sex { get; set; }

        [Column("Orgprs.idnum")]
        //CodeMeli
        public string IdNum { get; set; }

        [Column("Orgprs.BirthDate")]
        public DateTime BirthDate { get; set; }

        [Column("Orgprs.idnumber")]
        //shenasname
        public string IdNumber { get; set; }

        [Column("Orgprs.idserial")]
        public string IdSerial { get; set; }

        [Column("Orgprs.Marrage")]
        public bool Marrage { get; set; }

        [Column("Orgprs.Child")]
        public int Child { get; set; }

        [Column("Orgprs.insuranceId")]
        public string InsuranceCode { get; set; }

        [Column("Orgprs.Type")]
        public string Type { get; set; }

        [Column("Orgprs.TypeYear")]
        public string TypeYear { get; set; }

        [Column("Orgprs.Address")]
        public string Address { get; set; }

        [Column("Orgprs.Tell")]
        public string Tell { get; set; }

        [Column("Orgprs.Phone")]
        public string Phone { get; set; }

        [Column("Orgprs.Email")]
        public string Email { get; set; }



        [Column("Orgprs.ChartCode")]
        public int OrgChartId { get; set; }

        [Column("Orgprs.Salary")]
        public int SalaryId { get; set; }

        [Column("Orgprs.edu")]
        public int EducationId { get; set; }

        [Column("Orgprs.insuranceTyp")]
        public int InsuranceId { get; set; }



        [ForeignKey("OrgChartId")]
        public virtual OrgChart OrgChart { get; set; }

        [ForeignKey("SalaryId")]
        public virtual Salary Salary { get; set; }

        [ForeignKey("InsuranceId")]
        public virtual Insurance Insurance { get; set; }

        [ForeignKey("EducationId")]
        public virtual Education Education { get; set; }


        public virtual IList<Teacher> Teachers { get; set; }


        public bool haveTeachers
        {
            get
            {
                if (Teachers == null)
                {
                    return false;
                }

                return Teachers.Any();
            }
        }

        public bool isPersonHaveAllValue
        {
            get
            {
                var valList = new List<bool>();

                if (string.IsNullOrWhiteSpace(InsuranceCode))
                    valList.Add(true);

                if (string.IsNullOrWhiteSpace(IdSerial))
                    valList.Add(true);

                if (string.IsNullOrWhiteSpace(Address))
                    valList.Add(true);

                if (string.IsNullOrWhiteSpace(Tell))
                    valList.Add(true);

                if (string.IsNullOrWhiteSpace(Email))
                    valList.Add(true);

                return !valList.Any();

            }
        }
    }
}