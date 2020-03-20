using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Writer
    {
        public Writer() { }

        public int Id { get; set; }

        public string FullName { get; set; }

        public string Desc { get; set; }

        
        public string PicUrl { get; set; }
		[NotMapped]
		public string PicData { get; set; }
		[NotMapped]
		public string PicName { get; set; }
        
        public int Clap { get; set; }

        public virtual IList<Product> Products { get; set; }

        
        public bool haveAnyProduct
        {
            get
            {
                if (Products == null)
                {
                    return false;
                }

                return Products.Any();
            }
        }
    }
}