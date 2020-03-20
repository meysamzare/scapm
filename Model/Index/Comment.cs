using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model.Index
{
	[Table("Ind.Comment")]
    public class Comment
    {
        public Comment() { }

		public int Id { get; set; }

		public int PostId { get; set; }

		public string FullName { get; set; }

		public string Email { get; set; }

		public string Ip { get; set; }

		public DateTime Date { get; set; }

		public string Content { get; set; }

		public int? ParentId { get; set; }

		public bool HaveComformed { get; set; }


		[ForeignKey("PostId")]
		public virtual Post Post { get; set; }

		[ForeignKey("ParentId")]
		public virtual Comment Parent { get; set; }


		public virtual IList<Comment> Childrens { get; set; }



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

        public string postTitleString
        {
            get
            {
                if (Post == null)
                {
                    return "";
                }

                return Post.Title;
            }
        }

	}
}