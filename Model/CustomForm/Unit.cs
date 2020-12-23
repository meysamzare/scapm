using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model
{
    public class Unit
    {
        public Unit() { }

        public int Id { get; set; }

        public string Title { get; set; }

        public string EnTitle { get; set; }

        public int Order { get; set; }


        public virtual IList<Item> Items { get; set; }

        public virtual IList<Attribute> Attributes { get; set; }



        public bool HaveItem
        {
            get
            {
                if (Items == null)
                {
                    return false;
                }

                return Items.Any();
            }
        }

        public bool HaveAttr
        {
            get
            {
                if (Attributes == null)
                {
                    return false;
                }

                return Attributes.Any();
            }
        }
    }
}
