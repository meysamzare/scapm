using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    [Table("sm.insTitute")]
    public class InsTitute
    {
        public InsTitute()
        {

        }

        [Key]
        [Column("ins.AutoNum")]
        public int Id { get; set; }

        [Column("ins.Code")]
        public int? TituteCode { get; set; }

        [Column("ins.Name")]
        public string Name { get; set; }

        [Column("ins.OrgCode")]
        public int OrgCode { get; set; }

        [Column("ins.OrgSection")]
        public string OrgSection { get; set; }

        [Column("ins.OrgSex")]
        public sex OrgSex { get; set; }

        [Column("ins.State")]
        public string State { get; set; }

        [Column("ins.City")]
        public string City { get; set; }

        [Column("ins.Address")]
        public string Address { get; set; }

        [Column("ins.PostCode")]
        public string PostCode { get; set; }

        [Column("ins.Tell")]
        public string Tell { get; set; }

        [Column("ins.Email")]
        public string Email { get; set; }

        [Column("ins.Desc")]
        public string Desc { get; set; }



        [ForeignKey("TituteCode")]
        public virtual InsTitute Parent { get; set; }
        public virtual IList<InsTitute> Children { get; set; }
        
        public virtual IList<StdClassMng> StdClassMngs { get; set; }



        public virtual IList<Grade> Grades { get; set; }

        public virtual IList<ClassBook> ClassBooks { get; set; }
        

        public bool HaveChildren
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

        public bool haveGrade
        {
            get
            {
                if (Grades == null)
                {
                    return false;
                }
                return Grades.Any();
            }
        }

        public string parentTitle
        {
            get
            {
                if (Parent == null || TituteCode == null)
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

    public enum sex
    {
        Male = 1,
        Female = 2
    }
}