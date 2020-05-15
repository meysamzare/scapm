using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class AttributeOption
    {
        public AttributeOption() { }


        public long Id { get; set; }

        public string Title { get; set; }

        public bool IsTrue { get; set; }



        public int AttributeId { get; set; }

        [ForeignKey("AttributeId")]
        public virtual Attribute Attribute { get; set; }
    }
}