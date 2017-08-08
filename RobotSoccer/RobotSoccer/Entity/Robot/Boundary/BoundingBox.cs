using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.Entity.Robot.Boundary
{
    public class BoundingBox
    {
        public BoundingBox(PointF upperLeftCorner, PointF lowerRightCorner)
        {
            UpperLeftCorner = upperLeftCorner;
            LowerRightCorner = lowerRightCorner;

            Width = Math.Abs(UpperLeftCorner.X - LowerRightCorner.X);
            Height = Math.Abs(UpperLeftCorner.Y - LowerRightCorner.Y);
        }

        public BoundingBox(float x1, float y1, float x2, float y2)
            : this(new PointF(x1, y2), new PointF(x2, y2)) { }

        public bool IsIntersectingOrTouching(BoundingBox otherBox)
        {
            return (Math.Abs(UpperLeftCorner.X - otherBox.UpperLeftCorner.X) * 2 <= (Width + otherBox.Width)) &&
                (Math.Abs(UpperLeftCorner.Y - otherBox.UpperLeftCorner.Y) * 2 <= (Height + otherBox.Height));
        }

        public PointF UpperLeftCorner
        {
            get
            {
                return upperLeftCorner;
            }

            set
            {
                upperLeftCorner = value;

                if (lowerRightCorner != null)
                {
                    Width = Math.Abs(UpperLeftCorner.X - LowerRightCorner.X);
                    Height = Math.Abs(UpperLeftCorner.Y - LowerRightCorner.Y);
                }
            }
        }

        public PointF LowerRightCorner
        {
            get
            {
                return lowerRightCorner;
            }

            set
            {
                lowerRightCorner = value;

                if (upperLeftCorner != null)
                {
                    Width = Math.Abs(UpperLeftCorner.X - LowerRightCorner.X);
                    Height = Math.Abs(UpperLeftCorner.Y - LowerRightCorner.Y);
                }
            }
        }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public PointF upperLeftCorner;
        public PointF lowerRightCorner;
    }
}
