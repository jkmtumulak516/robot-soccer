using System.Collections.Generic;

namespace RobotSoccer.FuzzyLogic
{
    public interface IFuzzyResults
    {
        FuzzyResult GetResult(string Name);

        FuzzyResult GetResult(int Index);

        double Defuzzify(IEnumerable<FuzzyResult> Results);

        IEnumerable<FuzzyResult> Results { get; }

        string Category { get; }
    }
}
