using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Item
    {
        public Item() { }

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public string Tags { get; set; }

        public long RahCode { get; set; }

        public DateTime? DateAdd { get; set; }

        public DateTime? DateEdit { get; set; }


        public string AuthorizedFullName { get; set; }
        public string AuthorizedUsername { get; set; }
        public CategoryAuthorizeState AuthorizedType { get; set; }


        public int UnitId { get; set; }

        public int CategoryId { get; set; }



        public virtual Unit Unit { get; set; }

        public virtual Category Category { get; set; }



        public virtual IList<ItemAttribute> ItemAttribute { get; set; }


        public string getTotalScore
        {
            get
            {
                if (ItemAttribute == null)
                {
                    return "0";
                }

                return ItemAttribute.ToList().Sum(c => double.Parse(c.scoreString)).ToString("#,#0");
            }
        }

        public double getTotalScoreDouble
        {
            get
            {
                if (ItemAttribute == null)
                {
                    return 0.0;
                }

                return ItemAttribute.ToList().Sum(c => double.Parse(c.scoreString));
            }
        }

        public double getTotalScoreFunction(IList<ItemAttribute> itemAttrs, bool calculateNegativeScore = false)
        {
            if (itemAttrs == null)
            {
                return 0.0;
            }

            var score = itemAttrs.ToList().Sum(c => double.Parse(c.scoreString));

            if (calculateNegativeScore)
            {
                var trueCount = itemAttrs.Where(c => c.getPaperState(c, c.Attribute) == ItemAttributePaperState.True).Count();
                var falseCount = itemAttrs.Where(c => c.getPaperState(c, c.Attribute) == ItemAttributePaperState.False).Count();
                var blankCount = itemAttrs.Where(c => c.getPaperState(c, c.Attribute) == ItemAttributePaperState.Blank).Count();


                var scorePrecent = (double)((trueCount * 3) - (falseCount)) / ((trueCount + falseCount + blankCount) * 3);

                score = Math.Round((scorePrecent * 100) / 5, 2);
            }


            return score;
        }

        public string UnitString => Unit == null ? "" : Unit.Title;

        public string CategoryString => Category == null ? "" : Category.Title;

        public int CategoryRoleAccess => Category == null ? 0 : Category.RoleAccess;

        public string DateAddPersian => DateAdd == null ? "" : DateAdd.Value.ToPersianDate();

        public string DateEditPersian => DateEdit == null ? "" : DateEdit.Value.ToPersianDate();


        public string getRating(List<Item> items, double score)
        {
            if (items != null && items.Any())
            {
                var scores = items.OrderByDescending(c => c.getTotalScoreDouble).Select(c => c.getTotalScoreDouble).Distinct().ToList();


                var rate = "";

                try
                {
                    rate = (scores.FindIndex(c => c == score) + 1).ToString();
                }
                catch
                {
                    rate = "---";
                }

                return rate;
            }

            return "";
        }

    }
}
