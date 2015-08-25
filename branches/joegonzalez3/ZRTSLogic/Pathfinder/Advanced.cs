using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

namespace Pathfinder
{
	
    /// <summary>
    /// Contains advanced pathfinding functions such as waypoint smoothing.
    /// </summary>
	class Advanced
	{
		/*
		 * public functions
		 */

        /// <summary>
        /// Removes waypoints between other waypoints that have a clear path between them; in effect, smoothing the path
        /// </summary>
        /// <param name="path">The path to be smoothed</param>
        /// <returns>The smoothed path as a reduced list of waypoints</returns>
		public static List<Cell> smooth(List<Cell> path)
		{
			// NOT YET IMPLEMENTED
			return path;
		}

		
		/*
		 * helper functions
		 */


	}
}
