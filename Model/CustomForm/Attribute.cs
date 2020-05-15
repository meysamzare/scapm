using System;
using System.Collections.Generic;
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

        public int CategoryId { get; set; }

        public int UnitId { get; set; }

        public string Desc { get; set; }

        public AttrType AttrType { get; set; }

        public int MaxFileSize { get; set; }

        public bool IsUniq { get; set; }

        public int Order { get; set; }


        public bool IsInClient { get; set; }
        public bool IsInShowInfo { get; set; }
        public bool IsInSearch { get; set; }

        public int OrderInInfo { get; set; }

        public string Placeholder { get; set; }
        public bool IsRequired { get; set; }


        public bool IsMeliCode { get; set; }


        public virtual Unit Unit { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<ItemAttribute> ItemAttribute { get; set; }


        public virtual List<AttributeOption> AttributeOptions { get; set; }

        public bool haveAnyOption => AttributeOptions == null ? false : AttributeOptions.Any();
        public bool haveAnyTrueOption => AttributeOptions == null ? false : AttributeOptions.Any(c => c.IsTrue);

        public double Score { get; set; }

        public int OrderInt
        {
            get
            {
                if (Order == 0)
                {
                    return 1;
                }

                return Order;
            }
        }

        public bool HaveItemAttr
        {
            get
            {
                if (ItemAttribute == null)
                {
                    return false;
                }

                return ItemAttribute.Any();
            }
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

            return 1;
        }

        public string CatTitle
        {
            get
            {
                if (Category == null)
                {
                    return "";
                }
                return Category.Title;
            }
        }
        public string UnitTitle
        {
            get
            {
                if (Unit == null)
                {
                    return "";
                }
                return Unit.Title;
            }
        }
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
        radiobutton = 10
    }
}

