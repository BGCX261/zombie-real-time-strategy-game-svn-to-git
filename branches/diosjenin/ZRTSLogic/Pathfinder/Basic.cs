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
        /// <param name="startNode">The starting Node</param>
        /// <param name="endNode">The ending Node</param>
        /// <returns>The path as a list of waypoints</returns>
		public static List<Node> findPath(NodeMap map, Node startNode, Node endNode)
		{
			/*
			 * Terminology
			 *		Gscore			cumulative calculated distance from the start Node to the given Node
			 *		Hscore			estimated distance from the given Node to the end Node.
			 *							Overestimating this can result in the calculation of an incorrect (inefficient) path,
			 *							but the more it is underestimated, the longer correct path calculation will take
			 *		Fscore			Gscore + Hscore; estimated total distance from start to end on a path through the given Node.
			 *							Priority queues (PQueues) are ordered by ascending Fscore, so shortest estimated paths are examined first
			 *		open list		A PQueue of Nodes to be examined.  Initially contains the start Node
			 *		closed list		A List<Node> of Nodes that have been examined
			 *		adjacent list	A PQueue of Nodes adjacent to the current Node
			 */

			// initialize the lists
			PQueue open = new PQueue();
			open.enqueue(startNode);
			List<Node> closed = new List<Node>();
			PQueue adjacentNodes = new PQueue();
            iterations = 0;

			// good ol' A*
			while (open.Count > 0)											    // iterate until we have examined every appropriate Node
			{
                //open.print("Open", true);
				Node currentNode = open.dequeue();								    // look at the Node with the lowest Fscore and remove it from the open list
				if (currentNode == endNode)										    // if this is our destination Node, we're done!
					return reconstruct(endNode);									    // so return the path
				closed.Add(currentNode);										    // otherwise, close this Node so we don't travel to it again
				adjacentNodes = getAdjacentNodes(map, open, closed, currentNode);	// now find every valid Node adjacent to the current Node
                //adjacentNodes.print("Adjacent", false);
				while (adjacentNodes.Count != 0)                                  // iterate over all of them, from lowest to highest Fscore
				{
					Node adjacentNode = adjacentNodes.dequeue();									    // grab the current adjacent Node
					int tempGScore = currentNode.Gscore + map.pathDistance(currentNode, adjacentNode);	// calculate a temporary Gscore as if we were traveling to this Node from the current Node
					if (!open.contains(adjacentNode) || tempGScore < adjacentNode.Gscore)			    // if this Node has not been added to the open list, or if tempGscore is less than the Node's current Gscore
					{
						int h = map.pathDistance(adjacentNode, endNode);									// estimate the Node's Hscore
						adjacentNode.prev = currentNode;												    // set the Node's prev pointer to the current Node
						adjacentNode.Hscore = h;														    // set the Node's Hscore
						adjacentNode.Gscore = tempGScore;												    // set the Node's Gscore
						adjacentNode.Fscore = tempGScore + h;											    // set the Node's Fscore
					}
					if (!open.contains(adjacentNode))												    // if the adjacent Node we just examined is not yet on the open list, add it
						open.enqueue(adjacentNode);
				}
                iterations++;
			}
			
			// no valid path exists, so find the nearest path
			closed.RemoveAt(0);										// remove the start Node from the closed List
			if (closed.Count > 0)									// if there are still Nodes on the closed list
			{
				Node nearestNode = closed[0];							// find the closed Node with the lowest Hscore;
				for (int i = 1; i < closed.Count; i++)					// this should be the Node closest to the desired destination,
				{														// so return a path ending with that Node.
					if (closed[i].Hscore < nearestNode.Hscore)
						nearestNode = closed[i];
				}
				return reconstruct(nearestNode);
			}
			else
			{
				List<Node> path = new List<Node>();					// otherwise, our only path was the start Node (i.e. we are completely trapped);
				path.Add(endNode);									// so return a path with just that Node.
				return path;
			}
		}


		/*
		 * helper functions
		 */

        /// <summary>
        /// Puts all valid Nodes adjacent to the given Node in a PQueue and returns it
        /// </summary>
        /// <param name="map">The Map</param>
        /// <param name="closed">The closed list</param>
        /// <param name="currentNode">The current (center) Node</param>
        /// <returns>A PQueue of all traversable adjacent Nodes</returns>
		private static PQueue getAdjacentNodes(NodeMap map, PQueue open, List<Node> closed, Node currentNode)
		{
			int x = currentNode.Xcoord;
			int y = currentNode.Ycoord;
            List<Node> immediate = new List<Node>();
            List<Node> diagonal = new List<Node>();
			PQueue adjacentNodes = new PQueue();

            // grab all adjacent Nodes (or null values) and store them here
            Node[,] temp = map.getNodes(x - 1, y - 1, 3, 3);
            
            // iterate over all adjacent Nodes; add the ones that are open and in bounds to the appropriate List<Node>
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (temp[i, j] != null && !closed.Contains(temp[i, j]))
                    {
                        // if the Node is horizontally or vertically adjacent,
                        // add the Node to the list of immediately adjacent Nodes
                        if (Math.Abs(2 - i - j) == 1)
                            immediate.Add(temp[i, j]);

                        // otherwise, if the Node is valid, add it to the list of diagonally adjacent Nodes
                        else if (temp[i, j].isValid)
                            diagonal.Add(temp[i, j]);
                    }
                }
            }

            // iterate over all immediately adjacent Nodes.  If they are valid, enqueue them;
            // otherwise, remove the neighboring diagonally adjacent Nodes from the diagonal List
            for (int i = 0; i < immediate.Count(); i++)
            {
                if (!immediate[i].isValid)
                {
                    Node one, two = null;
                    if (immediate[i].Xcoord == x)   // the Node is vertically adjacent
                    {
                        one = map.getNode(x + 1, immediate[i].Ycoord);
                        two = map.getNode(x - 1, immediate[i].Ycoord);
                    }
                    else                            // the Node is horizontally adjacent
                    {
                        one = map.getNode(immediate[i].Xcoord, y - 1);
                        two = map.getNode(immediate[i].Xcoord, y + 1);
                    }
                    if (one != null)
                        diagonal.Remove(one);
                    if (two != null)
                        diagonal.Remove(two);
                }
                else {
                    adjacentNodes.enqueue(immediate[i]);
                }
            }

            // enqueue all remaining diagonally adjacent Nodes
            for (int i = 0; i < diagonal.Count(); i++)
                adjacentNodes.enqueue(diagonal[i]);

            // return the finished PQueue
			return adjacentNodes;
		}
		
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
				int prevX = path[i].Xcoord - path[i - 1].Xcoord;
				int prevY = path[i].Ycoord - path[i - 1].Ycoord;
				int curX = endNode.Xcoord - path[i].Xcoord;
				int curY = endNode.Ycoord - path[i].Ycoord;
				if (theta(prevX, prevY) == theta(curX, curY))
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
