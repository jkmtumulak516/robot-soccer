using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public class FuzzyMetaData
    {
        public override bool Equals(object obj)
        {
            var newObj = obj as FuzzyValue;

            if (newObj != null)
            {
                return (Category.Equals(newObj.Category) &&
                    Name.Equals(newObj.Name) &&
                    Index == newObj.Index);
            }

            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string Category { get; set; }
        public string Name { get; set; }

        public int Index { get; set; }
    }
}
