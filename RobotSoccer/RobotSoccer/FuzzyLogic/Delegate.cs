using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public delegate double GetMembership(FuzzyValue value, double CrispValue);

    public delegate double GetArea(FuzzyResult result, double CrispValue);
}
