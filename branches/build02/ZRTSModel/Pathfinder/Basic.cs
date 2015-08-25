using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;
using System.Collections;

namespace Pathfinder
{
    /// <summary>
    /// Contains basic pathfinding functions.
    /// </summary>
	class Basic
	{

		/*
		 * public functions
		 */

        /// <summary>
        /// A*, modified to find a nearest path in case of failure
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="startNode">The starting Node</param>
        /// <param name="endNode">The ending Node</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Node> findPath(NodeMap map, Node startNode, Node endNode)
		{
			// initialize data
			PQueue open = new PQueue();
			open.enqueue(startNode);
			List<Node> adjacentNodes = new List<Node>();

			// good ol' A*
			while (open.Count > 0)
			{
				// find the open Node with the lowest Fscore and remove it from the open PQueue
				Node current = open.dequeue();

				// if this is our destination Node, we're done; otherwise, close it so we don't travel to it again
				if (current == endNode)
					return reconstruct(endNode);
				current.close();

				// find every valid Node adjacent to the current Node
				adjacentNodes = map.getAdjacentNodes(current);
				
				// iterate over all of them
				for (int i = 0; i < adjacentNodes.Count; i++)
				{
					// grab an adjacent Node and calculate a new GScore and HScore for it
					Node adjacent = adjacentNodes[i];
					int tempGScore = current.Gscore + map.pathDistance(current, adjacent);
					int tempHScore = map.pathDistance(adjacent, endNode);

					// if we have not opened this Cell, give it new stats and open it
					if (!adjacent.isOpen)
					{
						setAdjacentStats(adjacent, current, tempGScore, tempHScore);
						open.enqueue(adjacent);
					}

					// otherwise, if we have opened it but the new path to it is shorter than the old one, give it new stats and reset its position in the open PQueue
					else if (tempGScore < adjacent.Gscore)
					{
						setAdjacentStats(adjacent, current, tempGScore, tempHScore);
						//open.resetPosition(adjacent);
					}
				}
			}
			
			// no valid path exists, so find the nearest path
			List<Node> closed = createClosed(map);
			Node nearestNode = closed[0];

			// find the closed Node with the lowest Hscore (distance from the intended end); return a path to that Node
			if (closed.Count > 0)
			{
				for (int i = 1; i < closed.Count; i++)
				{
					if (closed[i].Hscore < nearestNode.Hscore)
						nearestNode = closed[i];
				}
			}
			return reconstruct(nearestNode);
		}


		/*
		 * helper functions
		 */

		#region pathfinder helper functions

		/// <summary>
		/// Sets a Cell's "adjacent stats."  Called whenever an adjacent Cell is added to the Open structure or otherwise updated.
		/// </summary>
		/// <param name="adjacent">The adjacent Cell to set/reset</param>
		/// <param name="current">The current Cell we are adjacent to</param>
		/// <param name="gScore">The adjacent Cell's calculated gScore</param>
		/// <param name="hScore">The adjacent Cell's calculated hScore</param>
		private static void setAdjacentStats(Node adjacent, Node current, int gScore, int hScore)
		{
			adjacent.prev = current;					// set the Cell's prev pointer to the current Cell
			adjacent.Hscore = hScore;					// set the Cell's Hscore
			adjacent.Gscore = gScore;					// set the Cell's Gscore
			adjacent.Fscore = gScore + hScore;			// set the Cell's Fscore
		}

		/// <summary>
		/// Iterates over the full Map and returns a closed List of Nodes
		/// </summary>
		/// <param name="map">The NodeMap to search</param>
		/// <returns>A List of all closed Nodes</returns>
		private static List<Node> createClosed(NodeMap map)
		{
			List<Node> closed = new List<Node>();
			for (int j = 0; j < map.height; j++)
			{
				for (int i = 0; i < map.width; i++)
				{
					Node temp = map.getNode(i, j);
					if (temp.isClosed)
						closed.Add(temp);
				}
			}
			return closed;
		}

		#endregion


		#region path reconstruction

		/// <summary>
        /// A wrapper function for path reconstruction
        /// </summary>
        /// <param name="endNode">The destination Node</param>
        /// <returns>The path as a list of waypoints</returns>
		private static List<Node> reconstruct(Node endNode)
		{
			List<Node> path = new List<Node>();
			return reconstruct(endNode, path);
		}

        /// <summary>
        /// Actual path reconstruction
        /// </summary>
        /// <param name="endNode">The ending Node</param>
        /// <param name="path">The (initially blank) path</param>
        /// <returns>The path as a list of waypoints</returns>
		private static List<Node> reconstruct(Node endNode, List<Node> path)
		{
            // recurse if necessary
			if (endNode.prev != null)
				path = reconstruct(endNode.prev, path);

			// if we are moving in the same direction we moved in last time,
			// there is no need to keep the previous Node in the path, so remove it
			if (path.Count >= 2)
			{
				int i = path.Count - 1;
				int prevX = path[i].X - path[i - 1].X;
				int prevY = path[i].Y - path[i - 1].Y;
				int curX = endNode.X - path[i].X;
				int curY = endNode.Y - path[i].Y;
				if (polarAngle(prevX, prevY) == polarAngle(curX, curY))
					path.RemoveAt(i);
			}

            // add the current Node and return
			path.Add(endNode);
			return path;
		}

        /// <summary>
        /// Returns the angle between the origin and the given point (thank you, Wikipedia)
        /// </summary>
        /// <param name="x">The X-coordinate</param>
        /// <param name="y">The Y-coordinate</param>
        /// <returns>The angle between the origin and the given (x, y)</returns>
        private static double polarAngle(int x, int y)
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

		#endregion


	}
}
