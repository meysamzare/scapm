using System.Collections.Generic;
using System.Linq;

namespace SCMR_Api.Model
{

    public class StudentType
    {
        public StudentType() { }


        public int Id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }


        public virtual IList<StdClassMng> StdClassMngs { get; set; }


        public bool haveStudent
        {
            get
            {
                if (StdClassMngs == null)
                {
                    return false;
                }

                return StdClassMngs.Any();
            }
        }
    }
}