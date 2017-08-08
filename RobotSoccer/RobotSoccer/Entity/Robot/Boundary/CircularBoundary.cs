using System.Drawing;
using System.Windows.Forms;

namespace RobotSoccer.Entity.Robot.Boundary
{
    public class CircularBoundary
    {
        public CircularBoundary(PointF position, float radius, Color boundaryColor)
        {
            Position = position;
            Radius = radius;

            BoundaryPen = new Pen(boundaryColor);

            UpdateBoundingBox();
        }

        public void Draw(PaintEventArgs e)
        {
            var drawPoint = new PointF(Position.X - Radius, Position.Y - Radius);

            var diameter = Radius * 2;

            var rectBoundary = new RectangleF(drawPoint, new SizeF(diameter, diameter));

            e.Graphics.DrawEllipse(BoundaryPen, rectBoundary);
        }

        public void Reposition(PointF position)
        {
            Position = position;
        }

        public void Resize(float radius)
        {

            UpdateBoundingBox();
        }

        private void UpdateBoundingBox()
        {
            var upperLeftCorner = new PointF(Position.X - Radius, Position.Y - Radius);
            var lowerRightCorner = new PointF(Position.X + Radius, Position.Y + Radius);

            BoundingBox = new BoundingBox(upperLeftCorner, lowerRightCorner);
        }


        public PointF Position { get; private set; }
        public float Radius { get; private set; }

        public BoundingBox BoundingBox { get; private set; }

        public Pen BoundaryPen { get; private set; }
    }
}
