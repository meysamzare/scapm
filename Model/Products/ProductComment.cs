using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class ProductComment
    {
        public ProductComment() { }
        

        public int Id { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string Ip { get; set; }

		public DateTime Date { get; set; }

		public string Content { get; set; }

		public int? ParentId { get; set; }

		public bool HaveComformed { get; set; }


		[ForeignKey("ParentId")]
		public virtual ProductComment Parent { get; set; }
		public virtual IList<ProductComment> Childrens { get; set; }


        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
		public virtual Product Product { get; set; }
        

        public string dateString
        {
            get
            {
                return Date.ToPersianDateWithTime();
            }
        }

        public bool haveChildren
        {
            get
            {
                if (Childrens == null)
                {
                    return false;
                }

                return Childrens.Any();
            }
        }

        public string productTitleString
        {
            get
            {
                if (Product == null)
                {
                    return "";
                }

                return Product.Title;
            }
        }
    }
}