using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model.Index
{
    [Table("Ind.Post")]
    public class Post
    {
        public Post()
        {

        }

        public int Id { get; set; }

        public string Author { get; set; }

        public DateTime DateCreate { get; set; }
        public DateTime DateEdited { get; set; }

        public string Content { get; set; }

        public string ShortContent { get; set; }

        public string Title { get; set; }

        public bool HaveComment { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public int ViewCount { get; set; }


        public string HeaderPicUrl { get; set; }
        [NotMapped]
        public string HeaderPicData { get; set; }
        [NotMapped]
        public string HeaderPicName { get; set; }


        public bool haveVideo { get; set; }


        public string Tags { get; set; }

        public string Url { get; set; }

        public long SumRating { get; set; }

        public int RatingCount { get; set; }

        public bool IsHighLight { get; set; }

        public bool IsActive { get; set; }

        public PostType Type { get; set; }



        public virtual IList<Comment> Comments { get; set; }
        public virtual IList<Schedule> Schedules { get; set; }
        public virtual IList<MainSlideShow> MainSlideShows { get; set; }


        public int commentCount
        {
            get
            {
                if (Comments == null)
                {
                    return 0;
                }

                return Comments.Count;
            }
        }

        public bool haveSchedules
        {
            get
            {
                if (Schedules == null)
                {
                    return false;
                }

                return Schedules.Any();
            }
        }

        public string dateCreateString
        {
            get
            {
                if (DateCreate < new DateTime(0622, 12, 30))
                {
                    return "";
                }

                return DateCreate.ToPersianDate();
            }
        }

        public string dateEditedString
        {
            get
            {
                if (DateEdited < new DateTime(0622, 12, 30))
                {
                    return "";
                }

                return DateEdited.ToPersianDate();
            }
        }
    }

    public enum PostType
    {
        feed = 1,
        post = 2,
        fadak = 3,
        amoozesh = 4,
        enzebati = 5,
        parvaresh = 6,
        mali = 7,
        it = 8,
        moshaver = 9,

        //--------------

        voroodBeSystem = 10,
        sabteNam = 11,
        mokatebat = 12,
        ghesmathayeSamane = 13,
        faq = 14,
        ehrazeHoviat = 15,
        sharayetSabteNam = 16,
        darkhastTajdidNazar = 17,
        enteghadVaPishnahad = 18,
        daneshAmoozan = 19,
        daneshAmookhtegan = 20,
        forsatHayeShoghli = 21,
        tamasBaTaha = 22,
        darbareTaha = 23,
        dabirKhaneBargozidegan = 24,
        hedayatTahsil = 25,

        //--------------
    }
}