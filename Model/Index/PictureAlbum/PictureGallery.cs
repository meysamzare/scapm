using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model.Index
{
    [Table("Ind.PictureGallery")]
    public class PictureGallery
    {
        public PictureGallery() { }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public DateTime CreateDate { get; set; }

        public int Like { get; set; }

        public int DisLike { get; set; }

        public int ViewCount { get; set; }

        public string Author { get; set; }


        public int PictureCount
        {
            get
            {
                if (Pictures == null)
                {
                    return 0;
                }

                return Pictures.Count;
            }
        }

        public string createDateString
        {
            get
            {
                return CreateDate.ToPersianDate();
            }
        }

        public string firstPicUrl
        {
            get
            {
                if (Pictures == null || !Pictures.Any())
                {
                    return "";
                }

                return Pictures.FirstOrDefault().PicUrl;
            }
        }


        public virtual IList<Picture> Pictures { get; set; }
    }
}