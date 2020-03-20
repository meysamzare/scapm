using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class Chat
	{
		public Chat()
		{

		}

		public int Id { get; set; }

		public int SenderId { get; set; }

		public int ReciverId { get; set; }

		public bool ReciveStatus { get; set; }

		public DateTime SendDate { get; set; }

		public DateTime? ReciveDate { get; set; }

		public string Text { get; set; }

		public bool FileStatus { get; set; }

		public string FileUrl { get; set; }
		[NotMapped]
		public string FileName { get; set; }
		[NotMapped]
		public string FileData { get; set; }


		public virtual User ReciverUser { get; set; }
		public virtual User SenderUser { get; set; }


		public string senderFullName
		{
			get
			{
				if (SenderUser == null)
				{
					return "";
				}

				return SenderUser.fullName;
			}
		}


		public string sendDateString
		{
			get
			{
				if (SendDate < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return SendDate.ToPersianDateWithTime();
			}
		}

		public string reciveDateString
		{
			get
			{
				if (ReciveDate.HasValue)
				{

					if (ReciveDate.Value < new DateTime(0622, 12, 30))
					{
						return "";
					}

					return ReciveDate.Value.ToPersianDateWithTime();
				}

				return "";
			}
		}
	}
}
