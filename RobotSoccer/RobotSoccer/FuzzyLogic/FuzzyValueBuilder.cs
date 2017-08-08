using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public class FuzzyValueBuilder
    {
        public static readonly int CEIL = 1;
        public static readonly int FLOOR = -1;
        public static readonly int DEFAULT = 0;


        public FuzzyValueBuilder()
        {
            InitializeValues(this);
        }

        private void InitializeValues(FuzzyValueBuilder builder)
        {
            InitializeId(builder);

            InitializeMetrics(builder);
        }

        private void InitializeId(FuzzyValueBuilder builder)
        {
            builder.name = string.Empty;
            builder.category = string.Empty;

            builder.index = 0;

            builder.autoIncrement = false;
        }

        private void InitializeMetrics(FuzzyValueBuilder builder)
        {
            builder.center = 0;
            builder.upperWidth = 0;
            builder.lowerWidth = 0;

            builder.leftModifier = 0;
            builder.rightModifier = 0;

            builder.membershipFunction = Linear;
        }

        public FuzzyValue Build()
        {
            var result = new FuzzyValue();

            result.Name = name;
            result.Category = category;

            result.Index = index;

            result.Center = center;
            result.UpperWidth = upperWidth;
            result.LowerWidth = lowerWidth;

            result.LeftModifier = leftModifier;
            result.RightModifier = rightModifier;

            result.GetMembership = membershipFunction;

            if (autoIncrement)
            {
                index++;
            }

            return result;
        }

        public FuzzyValueBuilder Reset()
        {
            InitializeValues(this);
            return this;
        }

        public FuzzyValueBuilder ResetId()
        {
            InitializeId(this);
            return this;
        }

        public FuzzyValueBuilder ResetMetrics()
        {
            InitializeMetrics(this);
            return this;
        }

        public FuzzyValueBuilder AutoIncrement(bool autoIncrement)
        {
            this.autoIncrement = autoIncrement;
            return this;
        }

        public FuzzyValueBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        public FuzzyValueBuilder Category(string category)
        {
            this.category = category;
            return this;
        }

        public FuzzyValueBuilder Index(int index)
        {
            this.index = index;
            return this;
        }

        public FuzzyValueBuilder Center(double center)
        {
            this.center = center;
            return this;
        }

        public FuzzyValueBuilder UpperWidth(double upperWidth)
        {
            this.upperWidth = upperWidth;
            return this;
        }

        public FuzzyValueBuilder LowerWidth(double lowerWidth)
        {
            this.lowerWidth = lowerWidth;
            return this;
        }

        public FuzzyValueBuilder LeftModifier(int leftModifier)
        {
            this.leftModifier = leftModifier;
            return this;
        }

        public FuzzyValueBuilder RightModifier(int rightModifier)
        {
            this.rightModifier = rightModifier;
            return this;
        }
        

        public FuzzyValueBuilder CustomMembership(GetMembership membershipFunction)
        {
            this.membershipFunction = membershipFunction;
            return this;
        }


        public FuzzyValueBuilder LinearMembership()
        {
            membershipFunction = Linear;
            return this;
        }

        private double Linear(FuzzyValue value, double CrispValue)
        {
            var distance = Math.Abs(value.Center - CrispValue);

            // CrispValue is the same as the value's center
            if (distance == 0)
            {
                return 1;
            }
            
            // if either side of the center is immediately raised to 0 or 100
            if (value.LeftModifier != 0 || value.RightModifier != 0)
            {
                var modifier = (value.Center > CrispValue ? value.LeftModifier : value.RightModifier);

                if (modifier != DEFAULT)
                {
                    return (modifier == CEIL ? 1 : 0);
                }
            }

            var upperDistance = value.UpperWidth / 2;
            var lowerDistance = value.LowerWidth / 2;

            // distance is within upper part of the trapezoid
            if (distance <= upperDistance)
            {
                return 1;
            }
            
            // distance is on the slope trapezoid
            else if (distance < lowerDistance)
            {
                var medianDistance = lowerDistance - upperDistance;

                return Math.Abs((lowerDistance - distance) / medianDistance);
            }

            // distance is outside the trapezoid
            else
            {
                return 0;
            }
        }


        private bool autoIncrement;

        private string name;
        private string category;

        private int index;

        private double center;
        private double upperWidth;
        private double lowerWidth;

        private int leftModifier;
        private int rightModifier;

        private GetMembership membershipFunction;
    }
}
