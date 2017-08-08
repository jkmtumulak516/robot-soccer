using System.Collections.Generic;

namespace RobotSoccer.FuzzyLogic
{
    public interface IFuzzyValues
    {
        FuzzyValue GetValue(string Name);

        FuzzyValue GetValue(int Index);

        ICollection<FuzzyValue> Fuzzify(double CrispValue);

        ICollection<FuzzyValue> Values { get; }

        string Category { get; }
    }
}
