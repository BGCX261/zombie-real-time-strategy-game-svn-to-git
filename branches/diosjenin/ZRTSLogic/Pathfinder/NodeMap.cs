using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

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

        public byte height;
        public byte width;
        private Node[,] nodes;


        /*
         * constructors
         */

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="m">The Gameworld.Map to copy dimensional and validity data from</param>
        public NodeMap(Map m)
        {
            this.height = m.height;
            this.width = m.width;
            this.nodes = new Node[width, height];
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    nodes[i, j] = new Node(m.getCell(i, j).Xcoord, m.getCell(i,j).Ycoord, m.getCell(i,j).isValid);
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
        /// <returns>A rough integer distance between the two; 10 per vertical/horizontal move, 14 per vertical move</returns>
        public int pathDistance(Node one, Node two)
        {
            /*
             * Theory
             *		This function is used to calculate the path-based distance between two Nodes;
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

    }
}
