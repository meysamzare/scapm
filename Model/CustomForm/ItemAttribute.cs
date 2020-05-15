using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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


        public bool isAttributeHaveOption => Attribute == null ? false : Attribute.haveAnyOption;


        [ForeignKey("AttributeId")]
        public virtual Attribute Attribute { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}
