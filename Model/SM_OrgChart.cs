using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.OrgChart")]
    public class OrgChart
    {
        public OrgChart()
        {

        }


        [Key]
        [Column("Org.Autonum")]
        public int Id { get; set; }


        [Column("Org.Code")]
        public int Code { get; set; }

        [Column("Org.Name")]
        public string Name { get; set; }

        [Column("Org.ParentCode")]
        public int? ParentId { get; set; }

        [Column("Org.Order")]
        public int Order { get; set; }

        [Column("Org.Desc")]
        public string Desc { get; set; }


        [ForeignKey("ParentId")]
        public virtual OrgChart Parent { get; set; }
        public virtual IList<OrgChart> Children { get; set; }


        public virtual IList<OrgPerson> OrgPeople { get; set; }


        public bool haveChildren
        {
            get
            {
                if (Children == null)
                {
                    return false;
                }
                return Children.Any();
            }
        }

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

        public string parentTitle
        {
            get
            {
                if (Parent == null || ParentId == null)
                {
                    return "ریشه";
                }
                else
                {
                    return Parent.Name;
                }
            }
        }
    }
}