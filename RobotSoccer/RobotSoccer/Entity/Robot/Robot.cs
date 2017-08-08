using RobotSoccer.Misc.Geometry;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RobotSoccer.Entity.Robot
{
    public abstract class Robot
    {
        public Robot(float initialX, float intiialY, float initialAngle, float accel,
            int interval, float maxDistancePerSecond, Color boundaryColor, Color directionalColor, Color turnignLineColor)
        {
            Center = new PointF(initialX, intiialY);
            Angle = new Angle(initialAngle);

            Accel = accel;

            MaxSpeed = maxDistancePerSecond * (interval / 1000f);

            BoundaryPen = new Pen(boundaryColor);
            DirectionalPen = new Pen(directionalColor);
            TurningLinePen = new Pen(turnignLineColor);

            RecentTurningPoint = Center;
        }


        protected void Move(float left, float right)
        {
            if (Math.Abs(left) == Math.Abs(right))
            {
                if (left == right)
                {
                    RecentTurningPoint = Center;

                    // passing left or right doesn't matter
                    // they're both equal in speed
                    StraightMove(left);
                }

                else
                {
                    var rotationFactor = GetPrimaryRotationFactor(left, right);

                    RecentTurningPoint = Center;

                    // passing left or right doesn't matter
                    // they're both equal in speed
                    TwistMove(Math.Abs(left), rotationFactor);
                }
            }

            else
            {
                var turningAngle = TurningPointAngle(left, right);
                var turningRadius = TurningPointDistance(left, right);
                var turningPoint = Geometry.FindPointFrom(Center, turningAngle, turningRadius);

                RecentTurningPoint = turningPoint;

                CurvedMove(turningPoint, turningAngle, turningRadius, left, right);
            }
        }


        public void Draw(PaintEventArgs e)
        {
            DrawBoundary(e);

            DrawDirectionalLine(e);

            DrawTurningPointLine(e);
        }

        protected void DrawDirectionalLine(PaintEventArgs e)
        {
            var frontPoint = Geometry.FindPointFrom(Center, Angle, Accel);
            var frontLine = new PointF[] { frontPoint, Center };

            e.Graphics.DrawLines(DirectionalPen, frontLine);
        }

        protected void DrawTurningPointLine(PaintEventArgs e)
        {

            var turningLine = new PointF[] { RecentTurningPoint, Center };

            e.Graphics.DrawLines(DirectionalPen, turningLine);
        }
        

        private Angle TurningPointAngle(float left, float right)
        {
            return new Angle(Angle.Degree + (Math.Abs(left) < Math.Abs(right) ? 90 : -90));
        }
        
        private float TurningPointDistance(float left, float right)
        {
            var absLeft = Math.Abs(left);
            var absRight = Math.Abs(right);

            if (Math.Sign(left) == Math.Sign(right) || left == 0 || right == 0)
            {
                var faster = (absLeft > absRight ? left : right);
                var slower = (absLeft < absRight ? left : right);

                return Math.Abs(Accel * (faster / (faster - slower)));
            }

            else
            {
                var faster = (absLeft > absRight ? absLeft : absRight);
                var slower = (absLeft < absRight ? absLeft : absRight);

                return Math.Abs(Accel * ((faster - slower) / faster));
            }
        }


        private void StraightMove(float speed)
        {
            var distance = GetTravelDistance(speed);

            var newCenter = Geometry.FindPointFrom(Center, Angle, distance);

            MoveRobot(newCenter, Angle);
        }

        private void TwistMove(float speed, int rotationFactor)
        {
            var curveAngle = GetAngleOfCurve(Center, Angle, Accel, GetTravelDistance(speed));

            var endAngle = new Angle(Angle.Degree + (curveAngle.Degree * rotationFactor));

            Angle = endAngle;
        }

        private void CurvedMove(PointF turningPoint, Angle firstTurningAngle, float turningRadius, float left, float right)
        {
            firstTurningAngle.Reverse();

            var absLeft = Math.Abs(left);
            var absRight = Math.Abs(right);

            var primaryRotationFactor = GetPrimaryRotationFactor(left, right);
            var secondaryRotationFactor = GetSecondaryRotationFactor(left, right);
            var tertiaryRotationFactor = GetTertiaryRotationFactor(left, right);
            var greaterSpeed = (absLeft > absRight ? absLeft : absRight);

            // difference between turn base angle and turn end angle
            var curveAngle = GetAngleOfCurve(turningPoint, firstTurningAngle, turningRadius + Accel, GetTravelDistance(greaterSpeed));

            var secondTurningAngle = new Angle(firstTurningAngle.Degree + (curveAngle.Degree * primaryRotationFactor));

            var endPoint = Geometry.FindPointFrom(turningPoint, secondTurningAngle, turningRadius);

            var endAngle = new Angle(secondTurningAngle.Degree + (90 * primaryRotationFactor * secondaryRotationFactor) + tertiaryRotationFactor);

            MoveRobot(endPoint, endAngle);
        }


        private int GetPrimaryRotationFactor(float left, float right)
        {
            if (left == right)
            {
                return 0;
            }

            else
            {
                return (left < right ? 1 : -1);
            }
        }

        private int GetSecondaryRotationFactor(float left, float right)
        {
            if (left < 0 || right < 0)
            {
                return -1;
            }

            else
            {
                return 1;
            }
        }

        private int GetTertiaryRotationFactor(float left, float right)
        {
            if (Math.Sign(left) != Math.Sign(right) && left != 0 && right != 0)
            {
                var absLeft = Math.Abs(left);
                var absRight = Math.Abs(right);

                var greater = (absLeft > absRight ? left : right);
                var lesser = (absLeft < absRight ? left : right);

                if (greater > 0)
                {
                    return 180;
                }

                else
                {
                    return 0;
                }
            }

            else
            {
                return 0;
            }
        }


        private float GetTravelDistance(float wheelSpeed)
        {
            return wheelSpeed * MaxSpeed;
        }

        private Angle GetAngleOfCurve(PointF turningPoint, Angle turningBaseAngle, float turningRadius, float targetDistance)
        {
            var turningDiameter = turningRadius * 2;

            return new Angle((360 * targetDistance) / ((float)Math.PI * turningDiameter));
        }

        private void MoveRobot(PointF newCenter, Angle newAngle)
        {
            Center = newCenter;

            Angle = newAngle;

            OnMove();
        }


        public abstract void Move();

        protected abstract void DrawBoundary(PaintEventArgs e);

        protected abstract void OnMove();


        public float Accel { get; protected set; }
        public Angle Angle { get; protected set; }
        public PointF Center { get; protected set; }
        protected PointF RecentTurningPoint { get; set; }

        protected float MaxSpeed { get; }
        
        protected Pen DirectionalPen { get; set; }
        protected Pen TurningLinePen { get; set; }
    }
}
