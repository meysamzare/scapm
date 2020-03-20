using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model.Index
{
    [Table("Ind.Advertising")]
    public class Advertising
    {
        public Advertising()
        {

        }

		public int Id { get; set; }

		public string Name { get; set; }

		public AdType Type { get; set; }

		public string Url { get; set; }

		public bool IsActive { get; set; }

		public string PicUrl { get; set; }


		
		[NotMapped]
		public string PicName { get; set; }
		[NotMapped]
		public string PicData { get; set; }


	}

	public enum AdType
	{
		fullRow = 1,
		halfRow = 2,
        special = 3,
        TMA = 4,
        PMA = 5,
        AllMobileApp = 6
	}
}