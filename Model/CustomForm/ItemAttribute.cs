using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;
using System;

namespace SCMR_Api.Model
{
    public class ItemAttribute
    {
        public ItemAttribute() { }

        [Key]
        public int Id { get; set; }

        public int AttributeId { get; set; }

        public int ItemId { get; set; }

        public string AttrubuteValue { get; set; }

        public string AttributeFilePath { get; set; }

        public double Score { get; set; }


        public bool isAttributeHaveOption => Attribute == null ? false : Attribute.haveAnyOption;


        [ForeignKey("AttributeId")]
        public virtual Attribute Attribute { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }



        public string scoreString
        {
            get
            {
                if (Attribute != null && Attribute.Question != null)
                {
                    return Math.Round(getScore(this, Attribute, Score), 2).ToString();
                }

                return "0.0";
            }
        }


        public double getScore(ItemAttribute itemAttr, Attribute attr, double score)
        {
            if (attr.AttrType == AttrType.combobox ||
                attr.AttrType == AttrType.radiobutton ||
                (attr.AttrType == AttrType.Question && attr.Question != null && attr.Question.Type == QueType.Test))
            {
                var attrOptions = attr.getAttributeOptions(true, attr.AttrType, attr.AttributeOptions, attr.Question == null || attr.Question.QuestionOptions == null ? new List<QuestionOption>() : attr.Question.QuestionOptions.ToList());

                if (attrOptions.Any() && attrOptions.Any(c => c.IsTrue))
                {
                    return attrOptions.Find(c => c.IsTrue).Id.ToString() == itemAttr.AttrubuteValue ? attr.Score : 0;
                }
            }

            return score;
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

        public bool isTrue(ItemAttribute itemAttr, Attribute attr)
        {
            if (attr.AttrType == AttrType.combobox ||
                attr.AttrType == AttrType.radiobutton ||
                (attr.AttrType == AttrType.Question && attr.Question != null && attr.Question.Type == QueType.Test))
            {
                var attrOptions = attr.getAttributeOptions(true, attr.AttrType, attr.AttributeOptions, attr.Question == null || attr.Question.QuestionOptions == null ? new List<QuestionOption>() : attr.Question.QuestionOptions.ToList());

                if (attrOptions.Any() && attrOptions.Any(c => c.IsTrue))
                {
                    return attrOptions.Find(c => c.IsTrue).Id.ToString() == itemAttr.AttrubuteValue;
                }
            }

            return false;
        }

        public bool isBlank(ItemAttribute itemAttr, Attribute attr)
        {
            return this.haveTrue(attr) && !this.isTrue(itemAttr, attr) && string.IsNullOrEmpty(itemAttr.AttrubuteValue);
        }

        public ItemAttributePaperState getPaperState(ItemAttribute itemAttr, Attribute attr)
        {
            if (this.haveTrue(attr))
            {
                var isTrue = this.isTrue(itemAttr, attr);
                var isBlank = this.isBlank(itemAttr, attr);

                if (isTrue && !isBlank)
                {
                    return ItemAttributePaperState.True;
                }
                if (!isTrue && !isBlank)
                {
                    return ItemAttributePaperState.False;
                }
                if (!isTrue && isBlank)
                {
                    return ItemAttributePaperState.Blank;
                }
            }

            return ItemAttributePaperState.None;
        }
    }
}

public enum ItemAttributePaperState
{
    None = 0,
    True = 1,
    False = 2,
    Blank = 3
}