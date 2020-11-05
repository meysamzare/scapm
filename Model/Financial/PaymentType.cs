using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model.Financial
{
    public class PaymentType
    {
        public PaymentType()
        {
			
        }

		[Column("PatmentType.Autonum")]
		public int Id { get; set; }

		[Column("PatmentType.Code")]
		public int Code { get; set; }

		[Column("PatmentType.Title")]
		public string Title { get; set; }

		[Column("PatmentType.Desc")]
		public string Desc { get; set; }


		public IList<StdPayment> StdPayments { get; set; }


		
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

	}
}