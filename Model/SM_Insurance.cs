using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.InsuranceTable")]
    public class Insurance
    {

        public Insurance()
        {
            
        }


        [Key]
        [Column("ins.Autonum")]
        public int Id { get; set; }


        [Column("ins.Name")]
        public string Name { get; set; }



        public virtual IList<OrgPerson> OrgPeople { get; set; }


        public bool havePerson
        {
            get
            {
                if (OrgPeople == null)
                {
                    return false;
                }
                return OrgPeople.Any();
            }
        }

    }
}