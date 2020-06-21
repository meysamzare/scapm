using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Category
    {
        public Category() { }

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

        public CategoryAuthorizeState AuthorizeState { get; set; }

        public string License { get; set; }

        public bool IsPined { get; set; }



        public CategoryTotalType Type { get; set; }

        public int? GradeId { get; set; }

        public int? ClassId { get; set; }

        public bool RandomAttribute { get; set; }

        public bool RandomAttributeOption { get; set; }

        
        public int[] TeachersIdAccess { get; set; }

        

        #region Relations

        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }

        public virtual IList<Category> Children { get; set; }
        public virtual IList<Item> Items { get; set; }
        public virtual IList<Attribute> Attributes { get; set; }


        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }

        #endregion


        #region CalculationProperties


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



        public bool haveLicense => !string.IsNullOrWhiteSpace(License);

        public bool HaveChildren => Children == null ? false : Children.Any();

        public bool haveAnyCodeMeliAttribute => Attributes == null ? false : Attributes.Any(c => c.IsMeliCode);
        public bool haveMoreThenOneCodeMeliAttribute => Attributes == null ? false : Attributes.Count(c => c.IsMeliCode) > 1;

        public string datePublishString => DatePublish.HasValue ? DatePublish.Value.ToPersianDateWithTime() : "---";

        public string dateExpireString => DateExpire.HasValue ? DateExpire.Value.ToPersianDateWithTime() : "---";

        public string parentTitle => ParentCategory == null || ParentId == null ? "ریشه" : ParentCategory.Title;

        public string getSplitedTitle => Title.Length > 20 ? Title.Substring(0, 20) + " ... " : Title;

        public string timePublish => DatePublish.HasValue ? DatePublish.Value.ToString("HH:mm") : "00:00";

        public string timeExpire => DateExpire.HasValue ? DateExpire.Value.ToString("HH:mm") : "00:00";

        public double formatedDateEnd => DateExpire.HasValue ? (DateExpire.Value - DateTime.Now).TotalSeconds : 0;

        public string gradeString => Grade == null ? "" : Grade.Name;
        public string classString => Class == null ? "" : Class.Name;

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

        #endregion


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

    public enum CategoryTotalType
    {
        registerForm = 0,
        onlineExam = 1,
    }
}
