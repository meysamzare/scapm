using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Education
    {
        public Education()
        {
            
        }


        [Key]
        [Column("edu.Autonum")]
        public int Id { get; set; }


        [Column("edu.Name")]
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