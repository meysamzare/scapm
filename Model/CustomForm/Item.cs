using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Item
    {
        public Item()
        {

        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public string Tags { get; set; }

        public long RahCode { get; set; }

        public DateTime? DateAdd { get; set; }

        public DateTime? DateEdit { get; set; }


        public string AuthorizedFullName { get; set; }
        public string AuthorizedUsername { get; set; }
        public CategoryAuthorizeState AuthorizedType { get; set; }


        public int UnitId { get; set; }

        public int CategoryId { get; set; }



        public virtual Unit Unit { get; set; }

        public virtual Category Category { get; set; }



        public virtual IList<ItemAttribute> ItemAttribute { get; set; }


        public string getTotalScore
        {
            get
            {
                if (ItemAttribute == null)
                {
                    return "0";
                }

                return ItemAttribute.ToList().Sum(c => double.Parse(c.scoreString)).ToString();
            }
        }

        public string UnitString => Unit == null ? "" : Unit.Title;

        public string CategoryString => Category == null ? "" : Category.Title;

        public int CategoryRoleAccess => Category == null ? 0 : Category.RoleAccess;

        public string DateAddPersian => DateAdd == null ? "" : DateAdd.Value.ToPersianDate();

        public string DateEditPersian => DateEdit == null ? "" : DateEdit.Value.ToPersianDate();

    }
}
