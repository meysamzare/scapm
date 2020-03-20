using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model.Financial
{
	[Table("Fin.Contract")]
	public class Contract
	{
		public Contract()
		{

		}

		public int Id { get; set; }

		public int Code { get; set; }

		public DateTime Date { get; set; }

		public string Title { get; set; }

		public int PartyContractId { get; set; }

		public string PartyContractName { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }

		public string FileUrl { get; set; }


		public int ContractTypeId { get; set; }

		[ForeignKey("ContractTypeId")]
		public virtual ContractType ContractType { get; set; }


		public virtual IList<StdPayment> StdPayments { get; set; }


		[NotMapped]
		public string FileData { get; set; }

		[NotMapped]
		public string FileName { get; set; }


		public bool haveStdPayments
		{
			get
			{
				if (StdPayments == null)
				{
					return false;
				}

				return StdPayments.Any();
			}
		}

		public string datePersian
		{
			get
			{

				if (Date < new DateTime(0622, 12, 30))
				{
					return "";
				}

				return Date.ToPersianDate();
			}
		}

	}
}