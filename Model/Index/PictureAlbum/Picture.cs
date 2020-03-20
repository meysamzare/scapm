using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model.Index
{
    [Table("Ind.Picture")]
    public class Picture
    {
        public Picture() { }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public int? PictureGalleryId { get; set; }

        

        public string PicUrl { get; set; }
        [NotMapped]
        public string PicData { get; set; }
        [NotMapped]
        public string PicName { get; set; }


        public string pictureGalleryName
        {
            get
            {
                if (PictureGallery == null)
                {
                    return "----";
                }

                return PictureGallery.Name;
            }
        }


        [ForeignKey("PictureGalleryId")]
        public virtual PictureGallery PictureGallery { get; set; }
    }
}