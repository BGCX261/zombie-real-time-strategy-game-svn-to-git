using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.GameModel.Scenario.Gameworld
{
    public class CircleShape : Shape
    {
        private int radius;

        public int Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public override bool Overlaps(Shape shape, PointF offset)
        {
            PointF negativeOffset = new PointF(-offset.X, -offset.Y);
            return shape.Overlaps(this, negativeOffset);
        }

        public override bool Overlaps(RectangleShape rect, PointF offset)
        {
            throw new NotImplementedException();
        }

        public override bool Overlaps(CircleShape circle, PointF offset)
        {
            throw new NotImplementedException();
        }
    }
}
