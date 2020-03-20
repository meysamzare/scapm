using System.Collections.Generic;
using System.Linq;

namespace SCMR_Api.Model
{
    public class ProductCategory
    {
        public ProductCategory() { }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Desc { get; set; }


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