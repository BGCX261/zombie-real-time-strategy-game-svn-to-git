using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;
using ZRTSModel;

namespace Pathfinder
{
    /// <summary>
    /// Public interface for pathfinding.
    /// </summary>
	public class FindPath
	{
        /*
         * private variables
         */

        static TimeSpan span;


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
		public static List<CellComponent> between(Map map, CellComponent start, CellComponent end, bool advanced)
		{
            // begin timing the operation
            DateTime startTime = DateTime.Now;

            // convert the given Cell-based data to Node-based data
            NodeMap nodeMap = new NodeMap(map);
            Node nodeStart = nodeMap.getNode(start.X, start.Y);
            Node nodeEnd = nodeMap.getNode(end.X, end.Y);

            // perform advanced pre-calculation tasks
            if (advanced)
            {
                // if the end Node is invalid, replace it with the nearest valid Node
                if (!nodeEnd.isValid)
                {
                    nodeEnd = Advanced.nearestValidEnd(nodeMap, nodeStart, nodeEnd);
                }
            }

            // find the path
			List<Node> nodePath = Basic.findPath(nodeMap, nodeStart, nodeEnd);

            // convert the path from List<Node> format back to List<Cell> format
            List<CellComponent> path = new List<CellComponent>(nodePath.Count);
            for (int i = 0; i < nodePath.Count; i++)
                path.Add(map.GetCellAt(nodePath[i].X, nodePath[i].Y));

            // grab and print path data
            float dist = (float)(nodePath[nodePath.Count - 1].Gscore);
            span = DateTime.Now - startTime;
            //printPath(path, dist);

			return path;
		}

        /// <summary>
        /// the basic function; requires only the map and the start and end Cells (advanced features are turned on)
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="start">The starting Cell</param>
        /// <param name="end">The ending Cell</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<CellComponent> between(Map map, CellComponent start, CellComponent end)
		{
			return between(map, start, end, true);
		}


        /*
         * helper functions
         */

        /// <summary>
        /// Prints the path to console.
        /// </summary>
        /// <param name="path">The path to print</param>
        private static void printPath(List<CellComponent> path, float distance)
		{
            CellComponent end = path[path.Count - 1];
            // bool intended = (intendedEnd == path[path.Count - 1]);
			// Console.WriteLine("-> {0} from ({1}, {2}) to ({3}, {4}) found", intended ? "Path" : "Nearest path", path[0].X, path[0].Y, intendedEnd.Xcoord, intendedEnd.Ycoord);
            Console.Write("-> Path: ");
			for (int i = 0; i < path.Count - 1; i++)
				Console.Write(String.Format("({0}, {1}), ", path[i].X, path[i].Y));
			Console.WriteLine(String.Format("({0}, {1})", end.X, end.Y));
            Console.WriteLine("    Waypoints: {0}", path.Count);
            Console.WriteLine("    Cell Distance: {0}", distance/10);
            Console.WriteLine("    Time to Find: {0}", span);
		}


	}
}
