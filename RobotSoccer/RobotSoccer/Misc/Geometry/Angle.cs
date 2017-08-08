using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotSoccer.Misc.Geometry
{
    public class Angle
    {
        public const int Q1 = 0;
        public const int Q2 = 1;
        public const int Q3 = 2;
        public const int Q4 = 3;
        public const int PX = 4;
        public const int PY = 5;
        public const int NX = 6;
        public const int NY = 7;

        public Angle(float degree)
        {
            Degree = degree;
        }


        public static Angle operator +(Angle lhs, Angle rhs)
        {
            return new Angle(lhs.Degree + rhs.Degree);
        }

        public static Angle operator -(Angle lhs, Angle rhs)
        {
            return new Angle(lhs.Degree - rhs.Degree);
        }

        public static Angle operator -(Angle angle)
        {
            return new Angle(-angle.degree);
        }

        public Angle RelativeAngle(Angle baseAngle)
        {
            return baseAngle - this;
        }

        public void Reverse()
        {
            Degree = Degree + 180;
        }

        private float Simplify(float angle)
        {
            while (angle >= 360 || angle < 0)
            {
                if (angle >= 360)
                {
                    angle -= 360;
                }

                else if (angle < 0)
                {
                    angle += 360;
                }
            }

            return angle;
        }


        public float Degree
        {
            get
            {
                return degree;
            }

            set
            {
                degree = Simplify(value);
                radian = degree * (float)(Math.PI / 180);
            }
        }

        public float Radian
        {
            get
            {
                return radian;
            }

            set
            {
                degree = Simplify(value * (float)(180 / Math.PI));
                radian = degree * (float)(Math.PI / 180);
            }
        }

        public int Orientation
        {
            get
            {
                if (degree > 0 && degree < 90)
                {
                    return Q1;
                }

                else if (degree > 90 && degree < 180)
                {
                    return Q2;
                }

                else if (degree > 180 && degree < 270)
                {
                    return Q3;
                }

                else if (degree > 270 && degree < 360)
                {
                    return Q4;
                }

                else if (degree == 0)
                {
                    return PX;
                }

                else if (degree == 90)
                {
                    return PY;
                }

                else if (degree == 180)
                {
                    return NX;
                }

                else
                {
                    return NY;
                }
            }
        }

        public Angle Opposite
        {
            get
            {
                return new Angle(Degree + 180);
            }
        }


        private float degree;
        private float radian;
    }
}
