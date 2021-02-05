using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model.Financial
{
	public class StdPayment
	{
		public StdPayment() { }

		[Column("StdPayment.Autonum")]
		public int Id { get; set; }

		[Column("StdPayment.Ref")]
		public string RefNum { get; set; }

		[Column("StdPayment.Bank")]
		public string Bank { get; set; }

		[Column("StdPayment.AccNum")]
		public string Hesab { get; set; }

		[Column("StdPayment.BankSection")]
		public string Shobe { get; set; }

		[Column("StdPayment.Amount", TypeName = "decimal(18, 2)")]
		public decimal Price { get; set; }





		[Column("StdPayment.PaymentTyp")]
		public int PaymentTypeId { get; set; }

		[ForeignKey("PaymentTypeId")]
		public virtual PaymentType PaymentType { get; set; }


		[Column("StdPayment.Student")]
		public int StudentId { get; set; }

		[ForeignKey("StudentId")]
		public virtual Student Student { get; set; }

		public int StdClassMngId { get; set; }
		
		[ForeignKey("StdClassMngId")]
		public virtual StdClassMng StdClassMng { get; set; }

		public int ContractId { get; set; }

		[ForeignKey("ContractId")]
		public virtual Contract Contract { get; set; }




		public string paymentTypeTitle
		{
			get
			{
				if (PaymentType == null)
				{
					return "";
				}
				return PaymentType.Title;
			}
		}

		public string contractTitle
		{
			get
			{
				if (Contract == null)
				{
					return "";
				}
				return Contract.Title;
			}
		}

		public string studentFullName
		{
			get
			{
				if (Student == null)
				{
					return "";
				}
				return Student.Name + " " + Student.LastName;
			}
		}

		public string priceString
		{
			get
			{
				return Price.ToString("#,##0");
			}
		}


	}
}