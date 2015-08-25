using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;
using ZRTSModel;

namespace Pathfinder
{
    /// <summary>
    /// A Map of Nodes; very similar to Gameworld.Map
    /// </summary>
    class NodeMap
    {
        /*
         * attributes
         */

        public int height;
        public int width;
		private Node[,] nodes;

		private const int IMMEDIATE_DISTANCE = 10;
		private const int DIAGONAL_DISTANCE = 14;


        /*
         * constructors
         */

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="m">The Gameworld.Map to copy dimensional and validity data from</param>
        public NodeMap(Map m)
        {
            this.height = m.GetHeight();
            this.width = m.GetWidth();
            this.nodes = new Node[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    nodes[i, j] = new Node(i, j, m.GetCellAt(i, j).GetTile().Passable() && !(m.GetCellAt(i, j).ContainsActiveEntities));
                }
            }
        }


        /*
         * public functions
         */

        /// <summary>
        /// Returns the Node at the given x and y coordinates
        /// </summary>
        /// <param name="x">X-coordinate</param>
        /// <param name="y">Y-coordinate</param>
        /// <returns>The Node if the coordinate is within map bounds, null otherwise</returns>
        public Node getNode(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= height)
                return null;
            return nodes[x, y];
        }

        /// <summary>
        /// Returns a Node[,] representation of a subsection of the map.  Coordinates outside map bounds are returned as null
        /// </summary>
        /// <param name="x">The starting X-coordinate</param>
        /// <param name="y">The starting Y-coordinate</param>
        /// <param name="width">Subsection width</param>
        /// <param name="height">Subsection height</param>
        /// <returns>A Node[,] subsection of the map</returns>
        public Node[,] getNodes(int x, int y, int width, int height)
        {
            Node[,] temp = new Node[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    temp[i, j] = getNode(i + x, j + y);
                }
            }
            return temp;
        }

        /// <summary>
        /// Calculates the minimum path-based distance between two Nodes
        /// </summary>
        /// <param name="one">The first Node</param>
        /// <param name="two">The second Node</param>
        /// <returns>A rough integer distance between the two; 10 per vertical/horizontal move, 14 per diagonal move</returns>
        public int pathDistance(Node one, Node two)
        {
            int x = Math.Abs(one.X - two.X);
            int y = Math.Abs(one.Y - two.Y);
            int diagonal = Math.Min(x, y);
            int straight = Math.Max(x, y) - diagonal;
            return (IMMEDIATE_DISTANCE * straight) + (DIAGONAL_DISTANCE * diagonal);
        }

		/// <summary>
		/// Puts all valid Nodes adjacent to the given Node in a List and returns it
		/// </summary>
		/// <param name="map">The Map</param>
		/// <param name="currentNode">The current (center) Node</param>
		/// <returns>A List of all traversable adjacent Nodes</returns>
		public List<Node> getAdjacentNodes(Node currentNode)
		{
			int x = currentNode.X;
			int y = currentNode.Y;
			List<Node> immediate = new List<Node>();
			List<Node> diagonal = new List<Node>();
			List<Node> adjacent = new List<Node>(8);

			// grab all adjacent Nodes (or null values) and store them here
			Node[,] temp = getNodes(x - 1, y - 1, 3, 3);

			// iterate over all adjacent Nodes; add the ones that are open and in bounds to the appropriate List<Node>
			for (int j = 0; j < 3; j++)
			{
				for (int i = 0; i < 3; i++)
				{
					if (temp[i, j] != null && !temp[i, j].isClosed)
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
					if (immediate[i].X == x)   // the Node is vertically adjacent
					{
						one = getNode(x + 1, immediate[i].Y);
						two = getNode(x - 1, immediate[i].Y);
					}
					else                            // the Node is horizontally adjacent
					{
						one = getNode(immediate[i].X, y - 1);
						two = getNode(immediate[i].X, y + 1);
					}
					if (one != null)
						diagonal.Remove(one);
					if (two != null)
						diagonal.Remove(two);
				}
				else
				{
					adjacent.Add(immediate[i]);
				}
			}

			// enqueue all remaining diagonally adjacent Nodes
			for (int i = 0; i < diagonal.Count(); i++)
				adjacent.Add(diagonal[i]);

			// return the finished PQueue
			return adjacent;
		}


	}
}
