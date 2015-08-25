using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

namespace Pathfinder
{
    /// <summary>
    /// Contains basic pathfinding functions.
    /// </summary>
	class Basic
	{
        public static int iterations;

		/*
		 * public functions
		 */

        /// <summary>
        /// A*, modified to find a nearest path in case of failure
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="startCell">The starting Cell</param>
        /// <param name="endCell">The ending Cell</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Cell> findPath(Map map, Cell startCell, Cell endCell)
		{
			/*
			 * Terminology
			 *		Gscore			cumulative calculated distance from the start Cell to the given Cell
			 *		Hscore			estimated distance from the given Cell to the end Cell.
			 *							Overestimating this can result in the calculation of an incorrect (inefficient) path,
			 *							but the more it is underestimated, the longer correct path calculation will take
			 *		Fscore			Gscore + Hscore; estimated total distance from start to end on a path through the given Cell.
			 *							Priority queues (PQueues) are ordered by ascending Fscore, so shortest estimated paths are examined first
			 *		open list		A PQueue of Cells to be examined.  Initially contains the start Cell
			 *		closed list		A List<Cell> of Cells that have been examined
			 *		adjacent list	A PQueue of Cells adjacent to the current Cell
			 */

			// initialize the lists
			PQueue open = new PQueue();
			open.enqueue(startCell);
			List<Cell> closed = new List<Cell>();
			PQueue adjacentCells = new PQueue();
            iterations = 0;

			// good ol' A*
			while (open.count() > 0)											    // iterate until we have examined every appropriate Cell
			{
                //open.print("Open", true);
				Cell currentCell = open.dequeue();								    // look at the Cell with the lowest Fscore and remove it from the open list
				if (currentCell == endCell)										    // if this is our destination Cell, we're done!
					return reconstruct(endCell);									    // so return the path
				closed.Add(currentCell);										    // otherwise, close this Cell so we don't travel to it again
				adjacentCells = getAdjacentCells(map, open, closed, currentCell);	// now find every valid Cell adjacent to the current Cell
                //adjacentCells.print("Adjacent", false);
				while (adjacentCells.count() != 0)                                  // iterate over all of them, from lowest to highest Fscore
				{
					Cell adjacentCell = adjacentCells.dequeue();									// grab the current adjacent Cell
					int tempGScore = currentCell.Gscore + pathDistance(currentCell, adjacentCell);	// calculate a temporary Gscore as if we were traveling to this Cell from the current Cell
					if (!open.contains(adjacentCell) || tempGScore < adjacentCell.Gscore)			// if this Cell has not been added to the open list, or if tempGscore is less than the Cell's current Gscore
					{
						int h = pathDistance(adjacentCell, endCell);									// estimate the Cell's Hscore
						adjacentCell.prev = currentCell;												// set the Cell's prev pointer to the current Cell
						adjacentCell.Hscore = h;														// set the Cell's Hscore
						adjacentCell.Gscore = tempGScore;												// set the Cell's Gscore
						adjacentCell.Fscore = tempGScore + h;											// set the Cell's Fscore
					}
					if (!open.contains(adjacentCell))												// if the adjacent Cell we just examined is not yet on the open list, add it
						open.enqueue(adjacentCell);
				}
                iterations++;
			}
			
			// no valid path exists, so find the nearest path
			closed.RemoveAt(0);										// remove the start Cell from the closed List
			if (closed.Count > 0)									// if there are still Cells on the closed list
			{
				Cell nearestCell = closed[0];							// find the closed Cell with the lowest Hscore;
				for (int i = 1; i < closed.Count; i++)					// this should be the Cell closest to the desired destination,
				{														// so return a path ending with that Cell.
					if (closed[i].Hscore < nearestCell.Hscore)
						nearestCell = closed[i];
				}
				return reconstruct(nearestCell);
			}
			else
			{
				List<Cell> path = new List<Cell>();					// otherwise, our only path was the start Cell (i.e. we are completely trapped);
				path.Add(endCell);									// so return a path with just that Cell.
				return path;
			}
		}


		/*
		 * helper functions
		 */

        /// <summary>
        /// Puts all valid Cells adjacent to the given Cell in a PQueue and returns it
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="closed">The closed list</param>
        /// <param name="currentCell">The current (center) Cell</param>
        /// <returns>A PQueue of all traversable adjacent Cells</returns>
		private static PQueue getAdjacentCells(Map map, PQueue open, List<Cell> closed, Cell currentCell)
		{
			int x = currentCell.Xcoord;
			int y = currentCell.Ycoord;
            List<Cell> immediate = new List<Cell>();
            List<Cell> diagonal = new List<Cell>();
			PQueue adjacentCells = new PQueue();

            // grab all adjacent Cells (or null values) and store them here
            Cell[,] temp = map.getCells(x - 1, y - 1, 3, 3);
            
            // iterate over all adjacent Cells; add the ones that are open and in bounds to the appropriate List<Cell>
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (temp[i, j] != null && !closed.Contains(temp[i, j]))
                    {
                        // if the Cell is horizontally or vertically adjacent,
                        // add the Cell to the list of immediately adjacent Cells
                        if (Math.Abs(2 - i - j) == 1)
                            immediate.Add(temp[i, j]);

                        // otherwise, if the Cell is valid, add it to the list of diagonally adjacent Cells
                        else if (temp[i, j].isValid)
                            diagonal.Add(temp[i, j]);
                    }
                }
            }

            // iterate over all immediately adjacent Cells.  If they are valid, enqueue them;
            // otherwise, remove the neighboring diagonally adjacent Cells from the diagonal List
            for (int i = 0; i < immediate.Count(); i++)
            {
                if (!immediate[i].isValid)
                {
                    Cell one, two = null;
                    if (immediate[i].Xcoord == x)   // the Cell is vertically adjacent
                    {
                        one = map.getCell(x + 1, immediate[i].Ycoord);
                        two = map.getCell(x - 1, immediate[i].Ycoord);
                    }
                    else                            // the Cell is horizontally adjacent
                    {
                        one = map.getCell(immediate[i].Xcoord, y - 1);
                        two = map.getCell(immediate[i].Xcoord, y + 1);
                    }
                    if (one != null)
                        diagonal.Remove(one);
                    if (two != null)
                        diagonal.Remove(two);
                }
                else {
                    adjacentCells.enqueue(immediate[i]);
                }
            }

            // enqueue all remaining diagonally adjacent Cells
            for (int i = 0; i < diagonal.Count(); i++)
                adjacentCells.enqueue(diagonal[i]);

            // return the finished PQueue
			return adjacentCells;
		}

        /// <summary>
        /// Calculates the minimum path-based distance between two cells
        /// </summary>
        /// <param name="one">The first Cell</param>
        /// <param name="two">The second Cell</param>
        /// <returns>A rough integer distance between the two; 10 per vertical/horizontal move, 14 per vertical move</returns>
		private static int pathDistance(Cell one, Cell two)
		{
			/*
			 * Theory
			 *		This function is used to calculate the path-based distance between two cells;
			 *		i.e., the shortest possible path you can take from point A to point B
			 *		when you can only move at 45-degree and 90-degree angles.
			 *		The diagonal distance is multiplied by 14 instead of the square root of 2 (~1.41) to avoid using floating-point numbers;
			 *		the straight distance is multiplied by 10 to compensate.
			 */
            
			int x = Math.Abs(one.Xcoord - two.Xcoord);
			int y = Math.Abs(one.Ycoord - two.Ycoord);
			int diagonal = Math.Min(x, y);
			int straight = Math.Max(x, y) - diagonal;
			return (10 * straight) + (14 * diagonal);
		}
		
        /// <summary>
        /// A wrapper function for path reconstruction
        /// </summary>
        /// <param name="endCell">The destination Cell</param>
        /// <returns>The path as a list of waypoints</returns>
		private static List<Cell> reconstruct(Cell endCell)
		{
			List<Cell> path = new List<Cell>();
			return reconstruct(endCell, path);
		}

        /// <summary>
        /// Actual path reconstruction
        /// </summary>
        /// <param name="endCell">The ending Cell</param>
        /// <param name="path">The (initially blank) path</param>
        /// <returns>The path as a list of waypoints</returns>
		private static List<Cell> reconstruct(Cell endCell, List<Cell> path)
		{
            // recurse if necessary
			if (endCell.prev != null)
				path = reconstruct(endCell.prev, path);

			// if we are moving in the same direction we moved in last time,
			// there is no need to keep the previous Cell in the path, so remove it
			if (path.Count >= 2)
			{
				int i = path.Count - 1;
				int prevX = path[i].Xcoord - path[i - 1].Xcoord;
				int prevY = path[i].Ycoord - path[i - 1].Ycoord;
				int curX = endCell.Xcoord - path[i].Xcoord;
				int curY = endCell.Ycoord - path[i].Ycoord;
				if (theta(prevX, prevY) == theta(curX, curY))
					path.RemoveAt(i);
			}

            // add the current Cell and return
			path.Add(endCell);
			return path;
		}

        /// <summary>
        /// Returns the angle between the origin and the given point (thank you, Wikipedia)
        /// </summary>
        /// <param name="x">The X-coordinate</param>
        /// <param name="y">The Y-coordinate</param>
        /// <returns>The angle between the origin and the given (x, y)</returns>
        private static double theta(int x, int y)
        {
            if (x > 0)
                return Math.Atan(y / x);
            else if (x < 0 && y >= 0)
                return Math.Atan(y / x) + Math.PI;
            else if (x < 0 && y < 0)
                return Math.Atan(y / x) - Math.PI;
            else if (x == 0 && y > 0)
                return Math.PI / 2;
            else if (x == 0 && y < 0)
                return Math.PI - (Math.PI / 2);
            else
                return 0;
        }



	}
}
