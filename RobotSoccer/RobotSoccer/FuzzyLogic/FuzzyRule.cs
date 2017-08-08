using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public class FuzzyRule
    {
        public FuzzyRule(ISet<FuzzyValue> Values, FuzzyResult Result)
        {
            this.Values = Values;
            this.Result = Result;
        }

        public FuzzyRule(FuzzyValue[] Values, FuzzyResult Result)
        {
            this.Values = new HashSet<FuzzyValue>(Values);
            this.Result = Result;
        }

        public override bool Equals(object obj)
        {
            var newObj = obj as FuzzyRule;

            if (newObj != null)
            {
                return (Values.SetEquals(newObj.Values));
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

        public bool HasValue(string Category, string Name)
        {
            foreach (var value in Values)
            {
                if (value.Category.Equals(Category) && value.Name.Equals(Name))
                {
                    return true;
                }
            }

            return false;
        }

        public ISet<FuzzyValue> Values { get; internal set; }

        public FuzzyResult Result { get; internal set; }
    }
}
