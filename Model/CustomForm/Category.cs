using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Category
    {
        public Category()
        {

        }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public string Desc { get; set; }

        public DateTime? DatePublish { get; set; }

        public DateTime? DateExpire { get; set; }

        public bool IsActive { get; set; }

        public string EndMessage { get; set; }



        public bool HaveInfo { get; set; }

        public bool IsInfoShow { get; set; }

        public string ActiveMessage { get; set; }

        public string DeActiveMessage { get; set; }


        public int RoleAccess { get; set; }



        public bool HaveEntringCard { get; set; }

        public string BtnTitle { get; set; }



        public ShowRow ShowRow { get; set; }


        public int PostType { get; set; }


        public string RegisterPicUrl { get; set; }

        public string ShowInfoPicUrl { get; set; }

        public string HeaderPicUrl { get; set; }


        [NotMapped]
        public string RegisterFileData { get; set; }
        [NotMapped]
        public string RegisterFileName { get; set; }
        [NotMapped]
        public string ShowInfoFileData { get; set; }
        [NotMapped]
        public string ShowInfoFileName { get; set; }
        [NotMapped]
        public string HeaderPicData { get; set; }
        [NotMapped]
        public string HeaderPicName { get; set; }



        public CategoryAuthorizeState AuthorizeState { get; set; }

        public string License { get; set; }
        public bool haveLicense => !string.IsNullOrWhiteSpace(License);



        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }
        public virtual IList<Category> Children { get; set; }
        public virtual IList<Item> Items { get; set; }
        public virtual IList<Attribute> Attributes { get; set; }


        public bool HaveChildren
        {
            get
            {
                if (Children == null)
                {
                    return false;
                }
                return Children.Any();
            }
        }


        public string datePublishString
        {
            get
            {
                if (DatePublish.HasValue)
                {
                    return DatePublish.Value.ToPersianDateWithTime();
                }

                return "---";
            }
        }

        public string dateExpireString
        {
            get
            {
                if (DateExpire.HasValue)
                {
                    return DateExpire.Value.ToPersianDateWithTime();
                }

                return "---";
            }
        }

        public bool canShowByDate
        {
            get
            {
                if (IsActive)
                {
                    var nowDate = DateTime.Now;

                    if (nowDate >= DatePublish && nowDate < DateExpire)
                    {
                        return true;
                    }

                    return false;
                }

                return false;
            }
        }


        public string parentTitle
        {
            get
            {
                if (ParentCategory == null || ParentId == null)
                {
                    return "ریشه";
                }
                else
                {
                    return ParentCategory.Title;
                }
            }
        }

        public string getSplitedTitle
        {
            get
            {
                if (Title.Length > 20)
                {
                    return Title.Substring(0, 20) + " ... ";
                }
                else
                {
                    return Title;
                }
            }
        }

        public string timePublish
        {
            get
            {
                if (DatePublish.HasValue)
                {
                    return DatePublish.Value.ToString("HH:mm");
                }

                return "00:00";
            }
        }

        public string timeExpire
        {
            get
            {
                if (DateExpire.HasValue)
                {
                    return DateExpire.Value.ToString("HH:mm");
                }

                return "00:00";
            }
        }




    }

    public enum ShowRow
    {
        Up = 1,
        Down = 2
    }

    public enum CategoryAuthorizeState
    {
        none = 0,
        TMA = 1,
        PMA = 2,
        SMA = 3,
        All = 4
    }
}
