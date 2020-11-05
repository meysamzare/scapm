
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Salary
    {
        public Salary()
        {
            
        }

        
        [Key]
        [Column("sal.Autonum")]
        public int Id { get; set; }

        [Column("sal.Name")]
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