using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model.Financial
{
	[Table("Fin.ContractType")]
    public class ContractType
    {
        public ContractType()
        {

        }

		public int Id { get; set; }

		public string Title { get; set; }

		public ContractTypeTable Table { get; set; }

		public string Content { get; set; }

		public virtual IList<Contract> Contracts { get; set; }


		public bool haveContracts
		{
			get
			{
				if (Contracts == null) 
				{
					return false;
				}

				return Contracts.Any();
			}
		}

		
	}

	public enum ContractTypeTable
	{
		Student = 0,
		Person = 1,
		Other = 2
	}
}