using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public interface IFuzzyRuleSet
    {
        IEnumerable<FuzzyResult> Infer(IEnumerable<FuzzyValue> values);
    }
}
