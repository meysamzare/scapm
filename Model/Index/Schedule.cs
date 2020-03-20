using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model.Index
{
    [Table("Ind.Schedule")]
	public class Schedule
	{
		public Schedule()
		{

		}

		public int Id { get; set; }

		public int PostId { get; set; }

		public DateTime DateStart { get; set; }

		public DateTime DateEnd { get; set; }

		public string Title { get; set; }

		public string Content { get; set; }


		public string PicUrl { get; set; }
		[NotMapped]
		public string PicName { get; set; }
		[NotMapped]
		public string PicData { get; set; }



		[ForeignKey("PostId")]
		public virtual Post Post { get; set; }


		public bool isOver
		{
			get
			{
				if (DateStart < new DateTime(0622, 12, 30))
				{
					return true;
				}
				// && DateEnd.AddDays(1) > DateTime.Now
				if (DateTime.Now > DateEnd)
				{
					return true;
				}

				return false;
			}
		}

		public string dateStartString
		{
			get
			{
				if (DateStart < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return DateStart.ToPersianDate();
			}
		}

		public string dateEndString
		{
			get
			{
				if (DateEnd < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return DateEnd.ToPersianDate();
			}
		}



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


		public string formatedDateEnd
		{
			get
			{
				return DateEnd.ToString("yyyy-MM-dd HH:mm:ss");
			}
		}

	}
}