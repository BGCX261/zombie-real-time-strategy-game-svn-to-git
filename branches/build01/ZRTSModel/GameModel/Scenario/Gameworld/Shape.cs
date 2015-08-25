using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.GameModel.Scenario.Gameworld
{
    public abstract class Shape
    {
        /// <summary>
        /// Using a visitor like pattern, it lets the other shape know what shape it is and lets the other shape calculate whether or not there is an overlap.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public abstract bool Overlaps(Shape shape, PointF offset);

        /// <summary>
        /// Calculates if the other rectangle, with a center offset given, intersects with the given shape.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public abstract bool Overlaps(RectangleShape rect, PointF offset);

        /// <summary>
        /// Calculates if hte other circle, with a center offset given, intersects with the given shape.
        /// </summary>
        /// <param name="circle"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public abstract bool Overlaps(CircleShape circle, PointF offset);
    }
}
