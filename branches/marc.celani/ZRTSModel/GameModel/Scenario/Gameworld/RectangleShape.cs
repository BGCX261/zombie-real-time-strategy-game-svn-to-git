using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.GameModel.Scenario.Gameworld
{
    public class RectangleShape : Shape
    {
        private int width;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        private int height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public override bool Overlaps(Shape shape, PointF offset)
        {
            PointF negativeOffset = new PointF(-offset.X, -offset.Y);
            return shape.Overlaps(this, negativeOffset);
        }

        public override bool Overlaps(RectangleShape rect, PointF offset)
        {
            
            // Two rectangles, A and B, overlap iff one of the corners of B is contained in A or if A is completely contained within B.
            PointF a1 = new PointF(-rect.Width / 2 + offset.X, -rect.Height / 2 + offset.Y);
            bool overlaps = (a1.X >= -width / 2) && (a1.X <= width / 2) && (a1.Y >= -height / 2) && (a1.Y <= height / 2);
            if (!overlaps)
            {
                PointF a2 = new PointF(rect.Width / 2 + offset.X, -rect.Height / 2 + offset.Y);
                overlaps = (a2.X >= -width / 2) && (a2.X <= width / 2) && (a2.Y >= -height / 2) && (a2.Y <= height / 2);
                if (!overlaps)
                {
                    PointF a3 = new PointF(rect.Width / 2 + offset.X, rect.Height / 2 + offset.Y);
                    overlaps = (a3.X >= -width / 2) && (a3.X <= width / 2) && (a3.Y >= -height / 2) && (a3.Y <= height / 2);
                    if (!overlaps)
                    {
                        PointF a4 = new PointF(-rect.Width / 2 + offset.X, rect.Height / 2 + offset.Y);
                        overlaps = (a4.X >= -width / 2) && (a4.X <= width / 2) && (a4.Y >= -height / 2) && (a4.Y <= height / 2);
                        if (!overlaps)
                        {
                            overlaps = (-width / 2 >= a1.X) && (-width / 2 <= a2.X) && (-height / 2 >= a1.Y) && (-height / 2 <= a3.Y);
                        }
                    }
                }
            }
            return overlaps;
        }

        public override bool Overlaps(CircleShape circle, PointF offset)
        {
            throw new NotImplementedException();
        }
    }
}
