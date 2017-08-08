using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.Misc.Geometry
{
    public static class Geometry
    {
        public static PointF FindPointFrom(PointF basePoint, Angle angle, float distance)
        {
            float x = (float)(Math.Cos(angle.Radian) * distance);
            float y = (float)(Math.Sin(angle.Radian) * distance);

            x = basePoint.X + x;
            y = basePoint.Y - y;

            return new PointF(x, y);
        }

        public static float EuclideanDistance(PointF firstPoint, PointF secondPoint)
        {
            return (float)Math.Sqrt(SquaredDistance(firstPoint, secondPoint));
        }

        public static float SquaredDistance(PointF firstPoint, PointF secondPoint)
        {
            return (float)(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
        }
    }
}
