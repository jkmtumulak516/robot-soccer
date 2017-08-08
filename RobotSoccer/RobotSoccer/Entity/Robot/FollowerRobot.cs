using RobotSoccer.Entity.Robot.Boundary;
using RobotSoccer.FuzzyLogic;
using RobotSoccer.Misc.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotSoccer.Entity.Robot
{
    public class FollowerRobot : Robot
    {
        private const float WIDTH = 30f;
        private const float HEIGHT = 30f;

        private const float INITIAL_ANGLE = 90;
        private const float ACCEL = WIDTH / 2;

        private const float MAX_DISTANCE_PER_SECOND = 250f;

        // primary constructor
        public FollowerRobot(float initialX, float initialY, int interval)
            : base(initialX, initialY, INITIAL_ANGLE, ACCEL, interval,
                  MAX_DISTANCE_PER_SECOND, Color.Red, Color.Blue)
        {
            Boundary = new RectangularBoundary(Center, Angle, WIDTH, HEIGHT, Color.Black);

            InitializeFuzzyEngine();
        }

        // default constructor
        public FollowerRobot(int interval) : this(0f, 0f, interval) { }

        public void InitializeFuzzyEngine()
        {
            var angleValues = new FuzzyAngle();
            var distanceValues = new FuzzyDistance();
            var wheelValues = new FuzzyWheelSpeed();

            var listOfValues = new List<IFuzzyValues>(2);
            listOfValues.Add(angleValues);
            listOfValues.Add(distanceValues);

            var leftRules = new LeftWheelRuleSet();
            var rightRules = new RightWheelRuleSet();

            leftFuzzyEngine = new FuzzySharp(leftRules, listOfValues, wheelValues);
            rightFuzzyEngine = new FuzzySharp(rightRules, listOfValues, wheelValues);
        }


        protected override void DrawBoundary(PaintEventArgs e)
        {
            Boundary.Draw(e);
        }

        protected override void OnMove()
        {
            Boundary.Reposition(Center, Angle);
        }

        public override void Move()
        {

        }

        public void Move(PointF ball)
        {
            var angleToPoint = FindAngle(Center, ball);

            var relativeAngle = (Angle - new Angle((float)angleToPoint)).Degree;

            if (relativeAngle > 180)
            {
                relativeAngle = -(180 - (relativeAngle % 180));
            }

            var relativeDistance = FindDistance(Center, ball);

            var input = new Dictionary<string, double>(2);

            input.Add("Angle", relativeAngle);
            input.Add("Distance", relativeDistance);

            var left = leftFuzzyEngine.Evaluate(input) / 100;
            var right = rightFuzzyEngine.Evaluate(input) / 100;

            Move((float)left, (float)right);

            //Console.WriteLine("angle to ball -> " + relativeAngle);
            //Console.WriteLine("distance to ball -> " + relativeDistance);
        }


        private PointF FindPointFrom(PointF basePoint, Angle angle, float distance)
        {
            float x = (float)(Math.Cos(angle.Radian) * distance);
            float y = (float)(Math.Sin(angle.Radian) * distance);

            x = basePoint.X + x;
            y = basePoint.Y - y;

            return new PointF(x, y);
        }

        private double FindAngle(PointF origin, PointF target)
        {
            if (origin.X == target.X)
            {
                if (origin.Y > target.Y)
                {
                    return 270;
                }

                else if (origin.Y < target.Y)
                {
                    return 90;
                }
            }

            var orientationFactor = (double)(origin.X < target.X ? 0 : 180);

            var deltaX = (origin.X - target.X);
            var deltaY = (origin.Y - target.Y);

            var result = Math.Atan(deltaY / deltaX);

            // convert to degress from radian
            result = result * 180 / Math.PI;

            result = orientationFactor - result;

            return result;
        }

        private double FindDistance(PointF point1, PointF point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }


        public RectangularBoundary Boundary { get; }


        private FuzzySharp leftFuzzyEngine;
        private FuzzySharp rightFuzzyEngine;
    }
}
