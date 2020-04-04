using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class Link
    {
        public Link()
        {
            Id = Guid.NewGuid();
        }


        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Desc { get; set; }

        public int Value { get; set; }


        public string FileUrl { get; set; }
        public bool HaveExternalUrl { get; set; }
		[NotMapped]
		public string FileData { get; set; }
		[NotMapped]
		public string FileName { get; set; }


        
        public int Like { get; set; }
        public int ViewCount { get; set; }
        public int DownloadTime { get; set; }
        

        public decimal Price { get; set; }


        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}