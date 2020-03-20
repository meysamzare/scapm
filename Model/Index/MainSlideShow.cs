using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model.Index
{
	[Table("Ind.MainSlideShow")]
	public class MainSlideShow
	{
		public MainSlideShow()
		{

		}

		public int Id { get; set; }

		public string Name { get; set; }

		public int Page { get; set; }

		public DateTime DatePublish { get; set; }

		public DateTime DateExpire { get; set; }

		public string Title { get; set; }

		public string Desc { get; set; }


		public int PostId { get; set; }

		public string imgUrl { get; set; }
		[NotMapped]
		public string imgData { get; set; }
		[NotMapped]
		public string imgName { get; set; }

		public bool ShowState { get; set; }



		[ForeignKey("PostId")]
		public virtual Post Post { get; set; }


		public string postName
		{
			get
			{
				if (Post == null)
				{
					return "";
				}

				return Post.Name;
			}
		}


		public string postUrl
		{
			get
			{
				if (Post == null)
				{
					return "";
				}

				return Post.Url;
			}
		}


		public string datePublishString
		{
			get
			{
				if (DatePublish < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return DatePublish.ToPersianDate();
			}
		}

		public string dateExpireString
		{
			get
			{
				if (DateExpire < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return DateExpire.ToPersianDate();
			}
		}

	}
}