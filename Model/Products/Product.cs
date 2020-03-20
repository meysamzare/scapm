using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class Product
    {
        public Product() { }


        public int Id { get; set; }

        public string Title { get; set; }

        public ProductType Type { get; set; }

        // Time in Minuts OR Number of pages
        public int Value { get; set; }

        public string Desc { get; set; }

        public decimal TotalPrice { get; set; }


        public ProductTotalType TotalType { get; set; }

        public bool HaveComment { get; set; }


        public string PicUrl { get; set; }
        [NotMapped]
        public string PicData { get; set; }
        [NotMapped]
        public string PicName { get; set; }


        public string Tags { get; set; }

        public int Like { get; set; }


        public virtual IList<Link> Links { get; set; }


        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual IList<ProductComment> Comments { get; set; }

        public string productCategoryString
        {
            get
            {
                if (ProductCategory == null)
                {
                    return "";
                }

                return ProductCategory.Title;
            }
        }

        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; }

        public string writerString
        {
            get
            {
                if (Writer == null)
                {
                    return "";
                }
                return Writer.FullName;
            }
        }

        public bool haveAnyLink
        {
            get
            {
                if (Links == null)
                {
                    return false;
                }

                return true;
            }
        }
    }

    public enum ProductType
    {
        Book = 0,
        Document = 1,
        Movie = 2,
        Voice = 3
    }

    public enum ProductTotalType
    {
        Docs = 0,
        VirtualLearn = 1
    }
}