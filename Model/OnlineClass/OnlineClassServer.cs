using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCMR_Api.Model
{
    public class OnlineClassServer
    {
        public OnlineClassServer() { }

        public int Id { get; set; }


        public string Name { get; set; }

        public string Url { get; set; }

        public string PrivateKey { get; set; }


        public virtual IList<OnlineClass> OnlineClasses { get; set; }
    }
}