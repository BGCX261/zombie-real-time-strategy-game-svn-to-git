using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameModel;
using ZRTSModel.GameModel.Scenario.Gameworld;

namespace ZRTSModel
{
    /// <summary>
    /// An entity is an object that takes up space in the game world.  An entity therefore has a PointLocation representing its center, and a shape representing its shape.
    /// Upon moving (or entering the model), it calculates which cells that it would be located within, and hit tests with all of the entities already in those cells.  If a collision
    /// takes place, the move or placement fails.
    /// </summary>
    public class Entity
    {
        private PointF pointLocation;

        /// <summary>
        /// The center point of the entity
        /// </summary>
        public PointF PointLocation
        {
            get { return pointLocation; }
            set { pointLocation = value; }
        }
        private Shape shape;

        internal Shape Shape
        {
            get { return shape; }
        }

        /// <summary>
        /// Determines if two entities will overlap each other.  This is called before committing any change to the model to ensure that the moving/new entity
        /// does not in any way overlap with any other entities on the map.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool Overlaps(Entity e)
        {
            PointF offset = new PointF(e.PointLocation.X - pointLocation.X, e.PointLocation.Y - pointLocation.Y);
            return shape.Overlaps(e.Shape, offset);
        }
    }
}
