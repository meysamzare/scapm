using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class StudentInfo
    {
        public StudentInfo()
        {

        }

        [Key]
        [Column("Stdinfo.Autonum")]
        public int Id { get; set; }

        [Column("Stdinfo.FatherName")]
        public string FatherName { get; set; }

        [Column("Stdinfo.FatherEdu")]
        public string FatherEdu { get; set; }

        [Column("Stdinfo.FatherJob")]
        public string FatherJob { get; set; }

        [Column("Stdinfo.FatherJobPhone")]
        public string FatherJobPhone { get; set; }

        [Column("Stdinfo.FatherPhone")]
        public string FatherPhone { get; set; }

        [Column("Stdinfo.FatherJobAddress")]
        public string FatherJobAddress { get; set; }

        [Column("Stdinfo.MomName")]
        public string MomName { get; set; }

        [Column("Stdinfo.MomEdu")]
        public string MomEdu { get; set; }

        [Column("Stdinfo.MomJob")]
        public string MomJob { get; set; }

        [Column("Stdinfo.MomJobPhone")]
        public string MomJobPhone { get; set; }

        [Column("Stdinfo.MomPhone")]
        public string MomPhone { get; set; }

        [Column("Stdinfo.MomJobAddress")]
        public string MomJobAddress { get; set; }

        [Column("Stdinfo.HomeAddress")]
        public string HomeAddress { get; set; }

        [Column("Stdinfo.HomePhone")]
        public string HomePhone { get; set; }

        [Column("Stdinfo.FamilyState")]
        public string FamilyState { get; set; }

        [Column("Stdinfo.Religion")]
        public string Religion { get; set; }

        [Column("Stdinfo.SocialNet")]
        public string SocialNet { get; set; }

        [Column("Stdinfo.Email")]
        public string Email { get; set; }

        [Column("Stdinfo.Desc")]
        public string Desc { get; set; }




        [Column("Stdinfo.Code")]
        public int StudentId { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }



        public bool isAllHaveValue
        {
            get
            {
                var valList = new List<bool>();

                if (string.IsNullOrWhiteSpace(FatherName))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(FatherEdu))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(FatherJob))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(FatherJobPhone))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(FatherPhone))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(FatherJobAddress))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(MomName))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(MomEdu))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(MomJob))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(MomJobPhone))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(MomPhone))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(MomJobAddress))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(HomeAddress))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(HomePhone))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(FamilyState))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(Religion))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(SocialNet))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(Email))
                    valList.Add(true);
                if (string.IsNullOrWhiteSpace(Desc))
                    valList.Add(true);

                return !valList.Any();
            }
        }
    }
}