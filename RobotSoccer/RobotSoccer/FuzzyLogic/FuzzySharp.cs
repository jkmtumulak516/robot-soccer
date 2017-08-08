using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public sealed class FuzzySharp
    {
        public FuzzySharp(IFuzzyRuleSet RuleSet, IEnumerable<IFuzzyValues> Values, IFuzzyResults Results)
        {
            this.RuleSet = RuleSet;
            this.Values = Values.ToDictionary(x => x.Category, x => x);
            this.Results = Results;
        }

        public double Evaluate(IDictionary<string, double> CrispValues)
        {
            var fuzzifiedValues = new List<FuzzyValue>();

            foreach (var CrispValue in CrispValues)
            {
                var temp = Values[CrispValue.Key].Fuzzify(CrispValue.Value);

                foreach (var value in temp)
                {
                    fuzzifiedValues.Add(value);
                }
            }

            var fuzzifiedResults = RuleSet.Infer(fuzzifiedValues);

            return Results.Defuzzify(fuzzifiedResults);
        }

        private IFuzzyRuleSet RuleSet { get; set; }
        private IDictionary<string, IFuzzyValues> Values { get; set; }
        private IFuzzyResults Results { get; set; }
    }
}
