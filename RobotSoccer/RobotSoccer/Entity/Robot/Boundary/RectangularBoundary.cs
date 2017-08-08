using RobotSoccer.Misc.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RobotSoccer.Entity.Robot.Boundary
{
    public class RectangularBoundary
    {
        public RectangularBoundary(PointF position, Angle orientation, float width, float height, Color boundaryColor)
        {
            Width = width;
            Height = height;

            BoundaryPen = new Pen(boundaryColor);

            UpdateCornerAngle();

            UpdateCornerDistance();

            Reposition(position, orientation);

            UpdateBoundingBox();
        }

        public void Draw(PaintEventArgs e)
        {
            var cornerPoints = new PointF[5] { firstCorner, secondCorner, thirdCorner, fourthCorner, firstCorner };

            e.Graphics.DrawLines(BoundaryPen, cornerPoints);
        }

        public void Reposition(PointF position, Angle orientation)
        {
            var leftAngle = new Angle(orientation.Degree + 90);
            var rightAngle = new Angle(orientation.Degree - 90);

            firstCorner = FindPointFrom(position, leftAngle - CornerAngle, CornerDistance);
            secondCorner = FindPointFrom(position, rightAngle + CornerAngle, CornerDistance);
            thirdCorner = FindPointFrom(position, rightAngle - CornerAngle, CornerDistance);
            fourthCorner = FindPointFrom(position, leftAngle + CornerAngle, CornerDistance);

            UpdateBoundingBox();
        }

        private void UpdateCornerAngle()
        {
            var deltaY = Height / 2;
            var deltaX = Width / 2;

            var result = Math.Atan(deltaY / deltaX);

            CornerAngle = new Angle((float)(result * 180 / Math.PI));
        }

        private void UpdateCornerDistance()
        {
            CornerDistance = (float)Math.Sqrt(Math.Pow(Width / 2, 2) + Math.Pow(Height / 2, 2));
        }

        private void UpdateBoundingBox()
        {
            var upperLeftX = firstCorner.X;
            var upperLeftY = firstCorner.Y;
            var lowerRightX = firstCorner.X;
            var lowerRightY = firstCorner.Y;

            foreach (var corner in Corners)
            {
                // check X
                if (corner.X < upperLeftX)
                {
                    upperLeftX = corner.X;
                }

                else if (corner.X > lowerRightX)
                {
                    lowerRightX = corner.X;
                }

                // check Y
                if (corner.Y < upperLeftY)
                {
                    upperLeftY = corner.Y;
                }

                else if (corner.Y > lowerRightY)
                {
                    lowerRightY = corner.Y;
                }
            }

            BoundingBox = new BoundingBox(
                new PointF(upperLeftX, upperLeftY),
                new PointF(lowerRightX, lowerRightY));
        }

        private PointF FindPointFrom(PointF basePoint, Angle angle, float distance)
        {
            float x = (float)(Math.Cos(angle.Radian) * distance);
            float y = (float)(Math.Sin(angle.Radian) * distance);

            x = basePoint.X + x;
            y = basePoint.Y - y;

            return new PointF(x, y);
        }


        public PointF firstCorner { get; private set; }
        public PointF secondCorner { get; private set; }
        public PointF thirdCorner { get; private set; }
        public PointF fourthCorner { get; private set; }

        public PointF[] Corners
        {
            get
            {
                return new PointF[4] { firstCorner, secondCorner, thirdCorner, fourthCorner };
            }
        }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public Angle CornerAngle { get; private set; }

        public float CornerDistance { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        public Pen BoundaryPen { get; private set; }
    }
}
