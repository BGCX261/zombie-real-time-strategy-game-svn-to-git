using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

namespace Pathfinder
{
    /// <summary>
    /// Public interface for pathfinding.
    /// </summary>
	public class findPath
	{
        /*
         * private variables
         */

        static DateTime startTime;
        static TimeSpan span;
        static Cell intendedEnd;


		/*
		 * public functions
		 */

        /// <summary>
        /// The core function; calls all valid auxiliary functions and returns the path (advanced features can be toggled)
        /// Includes a boolean toggle for use of advanced pathfinding functions
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="start">The starting Cell</param>
        /// <param name="end">The ending Cell</param>
        /// <param name="advanced"> A boolean toggle for advanced functions</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Cell> between(Map map, Cell start, Cell end, bool advanced)
		{
            startTime = DateTime.Now;
            intendedEnd = end;
			List<Cell> path = Basic.findPath(map, start, end);
			if (advanced)
			{
				
            }
            float dist = (float)(path[path.Count - 1].Gscore);
            map.clean();
            span = DateTime.Now - startTime;
            printPath(path, dist);
			return path;
		}

        /// <summary>
        /// the basic function; requires only the map and the start and end Cells (advanced features are turned on)
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="start">The starting Cell</param>
        /// <param name="end">The ending Cell</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Cell> between(Map map, Cell start, Cell end)
		{
			return between(map, start, end, true);
		}

        /// <summary>
        /// Alternative to the basic function; takes raw floating-point start and end coordinates instead of Cells.
        /// Maps the raw coordinates to the appropriate start and end Cells before proceeding (advanced features are turned on)
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="xStart">The starting X coordinate</param>
        /// <param name="yStart">The starting Y coordinate</param>
        /// <param name="xEnd">The ending X coordinate</param>
        /// <param name="yEnd">The ending Y coordinate</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Cell> between(Map map, float xStart, float yStart, float xEnd, float yEnd)
		{
			return between(map, xStart, yStart, xEnd, yEnd, true);
		}

        /// <summary>
        /// Alternative to the basic function; takes raw floating-point start and end coordinates instead of Cells.
        /// Maps the raw coordinates to the appropriate start and end Cells before proceeding (advanced features can be toggled)
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="xStart">The starting X coordinate</param>
        /// <param name="yStart">The starting Y coordinate</param>
        /// <param name="xEnd">The ending X coordinate</param>
        /// <param name="yEnd">The ending Y coordinate</param>
        /// <param name="advanced"> A boolean toggle for advanced functions</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Cell> between(Map map, float xStart, float yStart, float xEnd, float yEnd, bool advanced)
		{
			int x0 = (int)xStart;
			int y0 = (int)yStart;
			int x1 = (int)xEnd;
			int y1 = (int)yEnd;
			Cell start = map.getCell(x0, y0);
			Cell end = map.getCell(x1, y1);
			return between(map, start, end, advanced);
		}

		/// <summary>
		/// Prints the path to console.
		/// </summary>
		/// <param name="path">The path to print</param>
		private static void printPath(List<Cell> path, float distance)
		{
            Cell end = path[path.Count - 1];
            bool intended = (intendedEnd == path[path.Count - 1]);
			Console.WriteLine("-> {0} from ({1}, {2}) to ({3}, {4}) found", intended ? "Path" : "Nearest path", path[0].Xcoord, path[0].Ycoord, intendedEnd.Xcoord, intendedEnd.Ycoord);
            Console.Write("    Path: ");
			for (int i = 0; i < path.Count - 1; i++)
				Console.Write(String.Format("({0}, {1}), ", path[i].Xcoord, path[i].Ycoord));
			Console.WriteLine(String.Format("({0}, {1})", end.Xcoord, end.Ycoord));
            Console.WriteLine("    Waypoints: {0}", path.Count);
            Console.WriteLine("    Cell Distance: {0}", distance/10);
            Console.WriteLine("    Cells Examined: {0}", Basic.iterations);
            Console.WriteLine("    Time to Find: {0}", span);
		}


	}
}
