using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCMR_Api.Model
{
    public class Setting
    {
        public Setting()
        {
        }

        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}