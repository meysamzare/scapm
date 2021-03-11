using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SCMR_Api.Model
{
    public class Attribute
    {
        public Attribute()
        {
            GId = Guid.NewGuid();
        }

        public int Id { get; set; }

        public Guid GId { get; set; }

        public string Title { get; set; }

        public string Values { get; set; }

        public int? CategoryId { get; set; }

        public int UnitId { get; set; }

        public string Desc { get; set; }

        public AttrType AttrType { get; set; }

        public int MaxFileSize { get; set; }

        public bool IsUniq { get; set; }

        public int UniqLimitCount { get; set; } = 1;

        public int Order { get; set; }


        // public bool IsEditable { get; set; }


        public bool IsInClient { get; set; }
        public bool IsInShowInfo { get; set; }
        public bool IsInSearch { get; set; }


        public int OrderInInfo { get; set; }

        public string Placeholder { get; set; }
        public bool IsRequired { get; set; }

        public bool IsMeliCode { get; set; }

        public bool IsPhoneNumber { get; set; }


        public string RequiredErrorMessage { get; set; }
        public string UniqErrorMessage { get; set; }


        public double Score { get; set; }


        public int? QuestionId { get; set; }


        public bool IsTemplate { get; set; }


        #region Relations

        public virtual Unit Unit { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public virtual IList<ItemAttribute> ItemAttribute { get; set; }
        public virtual List<AttributeOption> AttributeOptions { get; set; }

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }

        #endregion




        #region CalculationProperties

        public bool haveAnyOption => AttributeOptions == null ? false : AttributeOptions.Any();
        public bool haveAnyTrueOption => AttributeOptions == null ? false : AttributeOptions.Any(c => c.IsTrue);

        public int OrderInt => Order == 0 ? 1 : Order;

        public bool HaveItemAttr => ItemAttribute == null ? false : ItemAttribute.Any();

        public string CatTitle => Category == null ? "" : Category.Title;

        public string UnitTitle => Unit == null ? "" : Unit.Title;

        public List<AttributeOption> getAttributeOptions(bool withIsTrue, AttrType attrType, List<AttributeOption> options, List<QuestionOption> questionOptions, bool showTitle = false)
        {
            var AttrOptions = new List<AttributeOption>();

            if (attrType == AttrType.Question && questionOptions.Any())
            {
                questionOptions.ForEach(op => AttrOptions.Add(new AttributeOption
                {
                    Id = op.Id,
                    Title = showTitle ? op.Title : op.Name,
                    IsTrue = withIsTrue ? op.IsTrue : false
                }));
            }
            else
            {
                if (options != null)
                {
                    AttrOptions = options.Select(c => new AttributeOption
                    {
                        Id = c.Id,
                        Title = c.Title,
                        IsTrue = withIsTrue ? c.IsTrue : false
                    }).ToList();
                }
            }

            return AttrOptions;
        }

        public string AttrTypeString
        {
            get
            {
                if (AttrType == AttrType.text)
                {
                    return "متن";
                }
                if (AttrType == AttrType.number)
                {
                    return "عدد";
                }
                if (AttrType == AttrType.date)
                {
                    return "تاریخ";
                }
                if (AttrType == AttrType.checkbox)
                {
                    return "چک باکس";
                }
                if (AttrType == AttrType.password)
                {
                    return "کلمه عبور";
                }
                if (AttrType == AttrType.combobox)
                {
                    return "لیست";
                }
                if (AttrType == AttrType.pic)
                {
                    return "تصویر";
                }
                if (AttrType == AttrType.file)
                {
                    return "فایل";
                }
                if (AttrType == AttrType.textarea)
                {
                    return "متن طولانی";
                }
                if (AttrType == AttrType.radiobutton)
                {
                    return "لیست گزینشی";
                }
                if (AttrType == AttrType.Question)
                {
                    return "بانک سوالات";
                }


                return "";
            }
        }
        public int AttrTypeInt
        {
            get
            {
                if (AttrType == AttrType.text)
                {
                    return 1;
                }
                if (AttrType == AttrType.number)
                {
                    return 2;
                }
                if (AttrType == AttrType.date)
                {
                    return 3;
                }
                if (AttrType == AttrType.checkbox)
                {
                    return 4;
                }
                if (AttrType == AttrType.password)
                {
                    return 5;
                }
                if (AttrType == AttrType.combobox)
                {
                    return 6;
                }
                if (AttrType == AttrType.pic)
                {
                    return 7;
                }
                if (AttrType == AttrType.file)
                {
                    return 8;
                }
                if (AttrType == AttrType.textarea)
                {
                    return 9;
                }
                if (AttrType == AttrType.radiobutton)
                {
                    return 10;
                }
                if (AttrType == AttrType.Question)
                {
                    return 11;
                }

                return 1;
            }
        }

        public int AttrTypeToInt(AttrType attrType)
        {
            if (attrType == AttrType.text)
            {
                return 1;
            }
            if (attrType == AttrType.number)
            {
                return 2;
            }
            if (attrType == AttrType.date)
            {
                return 3;
            }
            if (attrType == AttrType.checkbox)
            {
                return 4;
            }
            if (attrType == AttrType.password)
            {
                return 5;
            }
            if (attrType == AttrType.combobox)
            {
                return 6;
            }
            if (attrType == AttrType.pic)
            {
                return 7;
            }
            if (attrType == AttrType.file)
            {
                return 8;
            }
            if (attrType == AttrType.textarea)
            {
                return 9;
            }
            if (attrType == AttrType.radiobutton)
            {
                return 10;
            }
            if (attrType == AttrType.Question)
            {
                return 11;
            }

            return 1;
        }

        public string AttrTypeToString(AttrType attrType)
        {
            if (attrType == AttrType.text)
            {
                return "متن";
            }
            if (attrType == AttrType.number)
            {
                return "عدد";
            }
            if (attrType == AttrType.date)
            {
                return "تاریخ";
            }
            if (attrType == AttrType.checkbox)
            {
                return "چک باکس";
            }
            if (attrType == AttrType.password)
            {
                return "کلمه عبور";
            }
            if (attrType == AttrType.combobox)
            {
                return "لیست";
            }
            if (attrType == AttrType.pic)
            {
                return "تصویر";
            }
            if (attrType == AttrType.file)
            {
                return "فایل";
            }
            if (attrType == AttrType.textarea)
            {
                return "متن طولانی";
            }
            if (attrType == AttrType.radiobutton)
            {
                return "لیست گزینشی";
            }
            if (attrType == AttrType.Question)
            {
                return "بانک سوالات";
            }


            return "";
        }


        public bool haveTrue(Attribute attr)
        {
            if (attr.AttrType == AttrType.combobox ||
                attr.AttrType == AttrType.radiobutton ||
                (attr.AttrType == AttrType.Question && attr.Question != null && attr.Question.Type == QueType.Test))
            {
                var attrOptions = attr.getAttributeOptions(true, attr.AttrType, attr.AttributeOptions, attr.Question == null || attr.Question.QuestionOptions == null ? new List<QuestionOption>() : attr.Question.QuestionOptions.ToList());

                if (attrOptions.Any() && attrOptions.Any(c => c.IsTrue))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }

    public enum AttrType
    {
        text = 1,
        number = 2,
        date = 3,
        checkbox = 4,
        password = 5,
        combobox = 6,
        pic = 7,
        file = 8,
        textarea = 9,
        radiobutton = 10,
        Question = 11
    }
}

