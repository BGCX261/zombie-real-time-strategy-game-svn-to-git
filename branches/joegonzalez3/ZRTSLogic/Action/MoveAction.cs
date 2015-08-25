using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameWorld;
using ZRTSModel.Entities;
using Pathfinder;
using ZRTSLogic;

namespace ZRTSLogic.Action
{
    /// <summary>
    /// This class will represent a "move" action that a unit can make. It will be used to make the unit 
    /// move in the Gameworld.
    /// </summary>
    public class MoveAction : ActionCommand
    {
        float targetX, targetY;
        List<Cell> path;
        Cell targetCell; // The current cell being moved to.
        GameWorld gw;
        Entity entity;
        Unit unit;
        int cellIndex;
        bool waiting = false;
        byte ticksWaiting = 0;

        byte TICKS_PER_MOVE = 2;       // How many ticks per step in the move action
        byte WAIT_TICKS = 30;               // How many ticks to wait for another unit to move.
        byte ticksSinceLastMove = 0;

        /// <summary>
        /// This constructor will create a MoveAction to the given targetX, and targetY.
        /// </summary>
        /// <param name="targetX">X coordinate destination of the move action in game space.</param>
        /// <param name="targetY">Y coordinate destination of the move action in game space.</param>
        /// <param name="gw">The GameWorld that the move action is occurring in.</param>
        /// <param name="entity">The Entity being given the MoveAction. (Should only be given to Units)</param>
        public MoveAction(float targetX, float targetY, GameWorld gw, Entity entity)
        {
            this.actionType = ActionType.Move;
            this.targetY = targetY;
            this.targetX = targetX;
            this.entity = entity;
            this.gw = gw;
            unit = (Unit)entity;
            float startX = unit.x;
            float startY = unit.y;

            Console.WriteLine(targetX);

            if ((int)targetX >= gw.map.width || (int)targetX < 0 || (int)targetY >= gw.map.height || (int)targetY < 0)
            {
                // Invalid target position.
                path = new List<Cell>();
            }
            else
            {
                path = findPath.between(gw.map, gw.map.getCell((int)startX, (int)startY), gw.map.getCell((int)targetX, (int)targetY));

                // Don't bother with path if it is just to same cell.
                if (path.Count > 1)
                {
                    targetCell = path[1];
                    cellIndex = 1;
                }
                else
                {
                    path = new List<Cell>();
                }
            }

        }

        /// <summary>
        /// This function will perform the next step of the move action.
        /// </summary>
        /// <returns>Returns true if the move action has been completed, false otherwise.</returns>
        public override bool work()
        {
            // Zero length path, done moving.
            if (path.Count == 0)
            {
                return true;
            }

            // Enough ticks have occured, take next step in move.
            if (ticksSinceLastMove % TICKS_PER_MOVE == 0)
            {   
                ticksSinceLastMove = 1;
                return takeStepMiddle();
            }
            else
            {
                ticksSinceLastMove++;
            }
            return false;
        }

        /// <summary>
        /// Variation of the takeStep method. The unit will move to the center of the targetCell. (x + 0.5f, y + 0.5f)
        /// </summary>
        /// <returns>true if at the end of the path, false otherwise.</returns>
        private bool takeStepMiddle()
        {
            // Check if we are at the center of the target cell.
            if (unit.x == targetCell.Xcoord + 0.5f && unit.y == targetCell.Ycoord + 0.5f)
            {
                // Are we at the last cell?
                if (cellIndex + 1 == path.Count)
                {
                    // At end of path, return true to indicate that we are done.
                    return true;
                }
                else
                {
                    // Not at end of path, move onto next cell
                    cellIndex++;
                    targetCell = path[cellIndex];
                }
            }

            if (isNextCellVacant())
            {
                // Next cell is vacant.
                if (waiting)
                {
                    waiting = false;
                    ticksWaiting = 0;
                }
                unit.getState().setPrimaryState(State.PrimaryState.Moving); // Set unit's state to moving
                moveMiddle();
            }
            else
            {
                // Next cell is not vacant.
                waiting = true;
                ticksWaiting++;
                unit.getState().setPrimaryState(State.PrimaryState.Idle);

                // We've been waiting long enough, compute a new path.
                if (ticksWaiting % WAIT_TICKS == 0)
                {
                    path = findPath.between(gw.map, gw.map.getCell((int)unit.x, (int)unit.y), gw.map.getCell((int)targetX, (int)targetY));
                    if (path.Count > 1)
                    {
                        targetCell = path[1];
                        cellIndex = 1;
                    }
                    else
                    {
                        path = new List<Cell>();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Move the unit towards the center of targetCell.
        /// </summary>
        private void moveMiddle()
        {
            // Need to move along the diagonal.
            if (!(targetCell.Ycoord + 0.5 == unit.y) && !(targetCell.Xcoord + 0.5f == unit.x))
            {

                if (Math.Sqrt(Math.Pow(targetCell.Ycoord + 0.5f - unit.y, 2) + Math.Pow(targetCell.Xcoord + 0.5f - unit.x, 2)) <= unit.stats.speed)
                {
                    // We are within |unit.speed| of the targetCell's center, set unit's position to center.
                    unit.x = targetCell.Xcoord + 0.5f;
                    unit.y = targetCell.Ycoord + 0.5f;
                }
                else if (unit.x > targetCell.Xcoord + 0.5f && unit.y > targetCell.Ycoord + 0.5f)
                {
                    // Need to move left and up. (NW)
                    unit.x += -unit.stats.speed / 1.41f;
                    unit.y += -unit.stats.speed / 1.41f;

                }
                else if (unit.x > targetCell.Xcoord + 0.5f && unit.y <= targetCell.Ycoord + 0.5f)
                {
                    // Need to move left and down (SW)
                    unit.x += -unit.stats.speed / 1.41f;
                    unit.y += unit.stats.speed / 1.41f;
                }
                else if (unit.x < targetCell.Xcoord + 0.5f && unit.y > targetCell.Ycoord + 0.5f)
                {
                    // Need to move right and up (NE)
                    unit.x += unit.stats.speed / 1.41f;
                    unit.y += -unit.stats.speed / 1.41f;
                }
                else
                {
                    // Need to move right and down (SE)
                    unit.x += unit.stats.speed / 1.41f;
                    unit.y += unit.stats.speed / 1.41f;
                }

            }
            else if (unit.x > targetCell.Xcoord + 0.5f) // Need to move left (W)
            {
                // Are we within unit.speed of the target cell center?
                if (Math.Abs(unit.x - (targetCell.Xcoord + 0.5f)) > unit.stats.speed)
                {
                    unit.x -= unit.stats.speed;
                }
                else
                {
                    unit.x = targetCell.Xcoord + 0.5f;
                }

            }
            else if (unit.x < targetCell.Xcoord + 0.5f) // Need to move right (E)
            {
                if (Math.Abs(unit.x - (targetCell.Xcoord + 0.5f)) > unit.stats.speed)
                {
                    unit.x += unit.stats.speed;
                }
                else
                {
                    unit.x = targetCell.Xcoord + 0.5f;
                }

            }
            else if (unit.y > targetCell.Ycoord + 0.5f) // Need to move down (S)
            {
                if (Math.Abs(unit.y - (targetCell.Ycoord + 0.5f)) > unit.stats.speed)
                {
                    unit.y -= unit.stats.speed;
                }
                else
                {
                    unit.y = targetCell.Ycoord + 0.5f;
                }

            }
            else if (unit.y < targetCell.Ycoord + 0.5f) // Need to move up (N)
            {
                if (Math.Abs(unit.y - (targetCell.Ycoord + 0.5f)) > unit.stats.speed)
                {
                    unit.y += unit.stats.speed;
                }
                else
                {
                    unit.y = targetCell.Ycoord + 0.5f;
                }

            }
        }

        /// <summary>
        /// This function will check if the next cell that the unit is currently moving onto (NOTE: This is not the "tagretCell.")
        /// is empty or not.
        /// </summary>
        /// <returns></returns>
        private bool isNextCellVacant()
        {
            int curX = (int)Math.Floor(unit.x);
            int curY = (int)Math.Floor(unit.y);

            int nextX, nextY;

            if (targetCell.Ycoord < curY)
            {
                nextY = curY - 1;
            }
            else if (targetCell.Ycoord > curY)
            {
                nextY = curY + 1;
            }
            else
            {
                nextY = curY;
            }

            if (targetCell.Xcoord < curX)
            {
                nextX = curX - 1;
            }
            else if (targetCell.Xcoord > curX)
            {
                nextX = curX + 1;
            }
            else
            {
                nextX = curX;
            }

            // I'm not sure if this will ever be true, but if the next cell's coords come out to be the same as the unit's
            // current cell coords, evaluate to true;
            if (nextX == curX && nextY == curY)
            {
                return true;
            }
            // Check to make sure that the next cells coords exist within the map's boundaries.
            else if (nextX < 0 || nextX >= gw.map.width || nextY < 0 || nextY >= gw.map.height)
            {
                return false;
            }


            return gw.map.getCell(nextX, nextY).isValid;
        }
    }
}
