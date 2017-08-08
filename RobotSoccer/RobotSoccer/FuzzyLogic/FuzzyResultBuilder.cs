using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.FuzzyLogic
{
    public class FuzzyResultBuilder
    {
        public static readonly int FLOOR = -1;
        public static readonly int DEFAULT = 0;


        public FuzzyResultBuilder()
        {
            InitializeValues(this);
        }

        private void InitializeValues(FuzzyResultBuilder builder)
        {
            InitializeId(builder);

            InitializeMetrics(builder);
        }

        private void InitializeId(FuzzyResultBuilder builder)
        {
            builder.name = string.Empty;
            builder.category = string.Empty;

            builder.index = 0;

            builder.autoIncrement = false;
        }

        private void InitializeMetrics(FuzzyResultBuilder builder)
        {
            builder.center = 0;
            builder.upperWidth = 0;
            builder.lowerWidth = 0;

            builder.leftModifier = 0;
            builder.rightModifier = 0;

            builder.areaFunction = Linear;
        }

        public FuzzyResult Build()
        {
            var result = new FuzzyResult();

            result.Name = name;
            result.Category = category;

            result.Index = index;

            result.Center = center;
            result.UpperWidth = upperWidth;
            result.LowerWidth = lowerWidth;

            result.LeftModifier = leftModifier;
            result.RightModifier = rightModifier;

            result.GetArea = areaFunction;

            if (autoIncrement)
            {
                index++;
            }

            return result;
        }

        public FuzzyResultBuilder Reset()
        {
            InitializeValues(this);

            return this;
        }

        public FuzzyResultBuilder ResetId()
        {
            InitializeId(this);

            return this;
        }

        public FuzzyResultBuilder ResetMetrics()
        {
            InitializeMetrics(this);

            return this;
        }

        public FuzzyResultBuilder AutoIncrement(bool autoIncrement)
        {
            this.autoIncrement = autoIncrement;
            return this;
        }

        public FuzzyResultBuilder Name(string name)
        {
            this.name = name;
            return this;
        }

        public FuzzyResultBuilder Category(string category)
        {
            this.category = category;
            return this;
        }

        public FuzzyResultBuilder Index(int index)
        {
            this.index = index;
            return this;
        }

        public FuzzyResultBuilder Center(double center)
        {
            this.center = center;
            return this;
        }

        public FuzzyResultBuilder UpperWidth(double upperWidth)
        {
            this.upperWidth = upperWidth;
            return this;
        }

        public FuzzyResultBuilder LowerWidth(double lowerWidth)
        {
            this.lowerWidth = lowerWidth;
            return this;
        }

        public FuzzyResultBuilder LeftModifier(int leftModifier)
        {
            this.leftModifier = leftModifier;
            return this;
        }

        public FuzzyResultBuilder RightModifier(int rightModifier)
        {
            this.rightModifier = rightModifier;
            return this;
        }


        public FuzzyResultBuilder CustomMembership(GetArea membershipFunction)
        {
            this.areaFunction = membershipFunction;
            return this;
        }


        public FuzzyResultBuilder LinearMembership()
        {
            areaFunction = Linear;
            return this;
        }

        private double Linear(FuzzyResult result, double Membership)
        {
            var upperDistance = result.UpperWidth / 2;
            var lowerDistance = result.LowerWidth / 2;
            var medianDistance = lowerDistance - upperDistance;

            var value = Membership;
            //var trueValue = value * 100;

            double leftArea = 0;
            double rightArea = 0;

            // TODO: Optimize

            // find area of left side
            if (result.LeftModifier != FLOOR)
            {
                var rectangle = value * ((medianDistance * (1 - value)) + upperDistance);
                var triangle = (value * (medianDistance * value)) / 2;

                leftArea = rectangle + triangle;
            }

            // find area of right side
            if (result.RightModifier != FLOOR)
            {
                var rectangle = value * ((medianDistance * (1 - value)) + upperDistance);
                var triangle = (value * (medianDistance * value)) / 2;

                rightArea = rectangle + triangle;
            }

            return leftArea + rightArea;
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

        private GetArea areaFunction;
    }
}
