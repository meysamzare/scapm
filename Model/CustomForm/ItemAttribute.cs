using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Collections.Generic;

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
                return getScore(this, Attribute, Score).ToString("#,#0");
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
    }
}
