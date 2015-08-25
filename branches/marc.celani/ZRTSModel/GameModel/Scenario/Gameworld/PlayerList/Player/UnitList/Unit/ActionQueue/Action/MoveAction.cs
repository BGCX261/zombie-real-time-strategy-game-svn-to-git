using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;
using Pathfinder;
using ZRTSModel;
using ZRTSModel.GameModel;

namespace ZRTSModel
{
    /// <summary>
    /// This class will represent a "move" action that a unit can make. It will be used to make the unit move in the Gameworld.
    /// </summary>
    public class MoveAction : EntityAction
    {
        public float targetX;
        public float targetY;
        private List<CellComponent> path = null;
        private Map map;
        private int cellIndex;
        private bool waiting = true;
		private UnitComponent unit;

		public bool Waiting { get { return waiting; } }
        private int ticksWaiting = 0; // Ticks spent waiting for another unit to move. 

        private byte TICKS_PER_MOVE = 5;       // How many ticks per step in the move action
        private byte WAIT_TICKS = 5;               // How many ticks to wait for another unit to move.
        private byte ticksSinceLastMove = 1;
        private float CENTER = 0.5f;            // Used to center the unit in its respective tile.


        /// <summary>
        /// This constructor will create a MoveAction to the given targetX, and targetY.
        /// </summary>
        /// <param name="targetX">X coordinate destination of the move action in game space.</param>
        /// <param name="targetY">Y coordinate destination of the move action in game space.</param>
        public MoveAction(float targetX, float targetY, Map map, UnitComponent unit)
        {
            this.targetY = targetY;
            this.targetX = targetX;
            this.map = map;
			this.unit = unit;
        }


        /// <summary>
        /// Variation of the takeStep method. The unit will move to the center of the targetCell. (x + 0.5f, y + 0.5f)
        /// </summary>
        /// <returns>true if at the end of the path, false otherwise.</returns>
        private bool takeStepMiddle(UnitComponent unit)
        {
            bool completed = false;

            // Check if we are at the center of the target Cell
            if (unit.PointLocation.X == path[0].X + CENTER && unit.PointLocation.Y == path[0].Y + CENTER)
            {
				// Remove the next Cell in the path.  If that was the last Cell, we're done
                path.RemoveAt(0);
                if (path.Count == 0)
                    completed = true;

            }

			// If we aren't at the center of the next Cell, and that Cell is vacant, move towards it
            else if (isNextCellVacant(unit))
            {
                if (waiting)
                {
                    waiting = false;
                    ticksWaiting = 0;
                }
                moveMiddle(unit);
            }

			// Otherwise, the next Cell in our path is not vacant; wait for a while or compute a new path
            else
            {
                waiting = true;
                ticksWaiting++;

                // We've been waiting long enough, compute a new path.
                if (ticksWaiting % WAIT_TICKS == 0)
                    path = FindPath.between(map, map.GetCellAt((int)unit.PointLocation.X, (int)unit.PointLocation.Y), map.GetCellAt((int)targetX, (int)targetY));
            }

            return completed;
        }

        /// <summary>
        /// Move the unit towards the center of targetCell.
        /// </summary>
        private void moveMiddle(UnitComponent unit)
        {
			float speed = (unit.Speed);

			if (path[0].Y + CENTER > unit.PointLocation.Y)
			{
				if (path[0].X + CENTER> unit.PointLocation.X)
				{
					unit.UnitOrient = UnitComponent.Orient.SE;
				}
				else if (path[0].X + CENTER < unit.PointLocation.X)
				{
					unit.UnitOrient = UnitComponent.Orient.SW;
				}
				else
				{
					unit.UnitOrient = UnitComponent.Orient.S;
				}
			}
			else if (path[0].Y + CENTER < unit.PointLocation.Y)
			{
				if (path[0].X + CENTER> unit.PointLocation.X)
				{
					unit.UnitOrient = UnitComponent.Orient.NE;
				}
				else if (path[0].X + CENTER< unit.PointLocation.X)
				{
					unit.UnitOrient = UnitComponent.Orient.NW;
				}
				else
				{
					unit.UnitOrient = UnitComponent.Orient.N;
				}
			}
			else if (path[0].X + CENTER< unit.PointLocation.X)
			{
				unit.UnitOrient = UnitComponent.Orient.W;
			}
			else if (path[0].X + CENTER> unit.PointLocation.X)
			{
				unit.UnitOrient = UnitComponent.Orient.E;
			}

            // If we are within the range of the destination point, simply move there
            if (Math.Sqrt(Math.Pow(path[0].Y + CENTER - unit.PointLocation.Y, 2) + Math.Pow(path[0].X + CENTER - unit.PointLocation.X, 2)) <= speed)
            {
                PointF directionVector = new PointF(path[0].X + CENTER - unit.PointLocation.X, path[0].Y + CENTER - unit.PointLocation.Y);
                // We are within |unit.speed| of the targetCell's center, set unit's position to center.
                unit.PointLocation = new PointF(path[0].X + CENTER, path[0].Y + CENTER);

            }
            else
            {
                PointF directionVector = new PointF(path[0].X + CENTER - unit.PointLocation.X, path[0].Y + CENTER - unit.PointLocation.Y);
                float magnitude = (float)Math.Sqrt(Math.Pow((double)directionVector.X, 2.0) + Math.Pow((double)directionVector.Y, 2.0));
                directionVector.X = directionVector.X / magnitude * unit.Speed;
                directionVector.Y = directionVector.Y / magnitude * unit.Speed;
                unit.PointLocation = new PointF(unit.PointLocation.X + directionVector.X, unit.PointLocation.Y + directionVector.Y);
            }
        }

        /// <summary>
		/// This function will check if the NEXT Cell that the unit is currently moving onto is empty or not.
        /// </summary>
        /// <param name="unit">The Unit whose location we check</param>
        /// <returns>True if the next Cell in its path is vacant, false otherwise</returns>
        private bool isNextCellVacant(UnitComponent unit)
        {
            int curX = (int)unit.PointLocation.X;
            int curY = (int)unit.PointLocation.Y;
			int nextX = curX;
			int nextY = curY;

            if (path[0].Y < curY)
                nextY = curY - 1;
            else if (path[0].Y > curY)
                nextY = curY + 1;

            if (path[0].X < curX)
                nextX = curX - 1;
            else if (path[0].X > curX)
                nextX = curX + 1;

            // I'm not sure if this will ever be true, but if the next cell's coords come out to be the same as the unit's
            // current cell coords, evaluate to true;
			if (nextX == curX && nextY == curY)
				return true;

			// Check to make sure that the next cells coords exist within the map's boundaries.
			else if (map.GetCellAt(nextX, nextY) == null)
				return false;

            return map.GetCellAt(nextX, nextY).EntitiesContainedWithin.Count == 0;
        }


		/// <summary>
		/// The Work function.  Creates a path if necessary, then moves along it.
		/// </summary>
		/// <returns>True once we have reached the destination, false otherwise.</returns>
        public override bool Work()
        {
			// This unit is now moving
			unit.State = UnitComponent.UnitState.MOVING;

			// If we have no path, create one
            if (path == null)
            {
                float startX = unit.PointLocation.X;
                float startY = unit.PointLocation.Y;
                path = FindPath.between(map, map.GetCellAt((int)startX, (int)startY), map.GetCellAt((int)targetX, (int)targetY));
				path.RemoveAt(0);
			}

            // Zero length path, done moving
            bool completed = true;
            if (path.Count != 0)
            {
				if (waiting)
				{
					if (ticksSinceLastMove % WAIT_TICKS == 0)
					{
						waiting = false;
						ticksSinceLastMove = 1;
					}
				}
				if (waiting == false && ticksSinceLastMove % TICKS_PER_MOVE == 0)
				{
					completed = takeStepMiddle(unit);
					waiting = true;
					ticksSinceLastMove = 0;
				}
				ticksSinceLastMove++;
				completed = false;
            }
            return completed;
        }
    }

}
