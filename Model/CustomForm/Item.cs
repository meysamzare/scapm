using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SCMR_Api.Model
{
	public class Item
	{
		public Item()
		{

		}

		[Key]
		public int Id { get; set; }

		[MaxLength(50)]
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



		public string UnitString
		{
			get
			{
				if (Unit == null)
				{
					return "";
				}

				return Unit.Title;
			}
		}

		public string CategoryString
		{
			get
			{
				if (Category == null)
				{
					return "";
				}

				return Category.Title;
			}
		}

		public int CategoryRoleAccess
		{
			get
			{
				if (Category == null)
				{
					return 0;
				}

				return Category.RoleAccess;
			}
		}

		public string DateAddPersian
		{
			get
			{
				if (DateAdd == null)
				{
					return "";
				}

				if (DateAdd.Value < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return DateAdd.Value.ToPersianDate();
			}
		}

		public string DateEditPersian
		{
			get
			{
				if (DateEdit == null)
				{
					return "";
				}

				if (DateEdit.Value < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return DateEdit.Value.ToPersianDate();
			}
		}
	}
}
