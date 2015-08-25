using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

namespace Pathfinder
{
    /// <summary>
    /// A custom-made priority queue for Nodes; ordered by ascending Fscore to promote efficient pathfinding.
    /// </summary>
	class PQueue
	{
		/*
		 * attributes
		 */

		private List<Node> pq;
        public int Count = 0;
        public bool hasLeadTie = false;


		/*
		 * constructors
		 */

        /// <summary>
        /// Blank constructor
        /// </summary>
		public PQueue()
		{
			pq = new List<Node>();
		}

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="p">The PQueue to copy</param>
        public PQueue(PQueue p)
        {
            this.pq.Clear();
            for (int i = 0; i < p.pq.Count; i++)
            {
                this.pq.Add(p.pq[i]);
            }
            recomputeStats();
        }


        /*
         * public functions
         */

        /// <summary>
        /// Removes the first Node in the PQueue and returns it
        /// </summary>
        /// <returns>The first Node in the PQueue</returns>
        public Node dequeue()
		{
            if (pq.Count == 0)
                return null;
			Node temp = pq[0];
			pq.RemoveAt(0);
            recomputeStats();
			return temp;
		}

        /// <summary>
        /// Inserts the given Node by its Fscore
        /// </summary>
        /// <param name="Node">The Node to insert</param>
		public void enqueue(Node node)
		{
            if (node == null)
                return;
			if (pq.Count == 0)
				pq.Add(node);
			else
			{
                int i = 0;
                while (i < pq.Count)
                {
                    if (node.Fscore >= pq[i].Fscore)
                        i++;
                    else
                        break;
                }
                pq.Insert(i, node);
			}
            recomputeStats();
		}

        /// <summary>
        /// Returns the Node at the given index without removing it
        /// </summary>
        /// <param name="i">The index to view</param>
        /// <returns>The Node at the given index</returns>
        public Node peek(int i)
        {
            if (Count <= i)
                return null;
            return pq[i];
        }

        /// <summary>
        /// Returns true if the PQueue contains the given Node, false otherwise
        /// </summary>
        /// <param name="Node">The Node to check for</param>
        /// <returns>True if the PQueue contains the given Node, false otherwise</returns>
		public bool contains(Node node)
		{
			return pq.Contains(node);
		}

        /// <summary>
        /// A debugging function; prints the PQueue's contents
        /// </summary>
        /// <param name="name">The string name of the list to print</param>
        /// <param name="includeF">Toggle for printing the Fscore of each Node after its coordinates</param>
        public void print(string name, bool includeF)
        {
            Console.Write("  {0}: ", name);
            for (int i = 0; i < Count; i++)
            {
                Console.Write("({0}, {1})", pq[i].Xcoord, pq[i].Ycoord);
                if (includeF)
                    Console.Write("+{0}", pq[i].Fscore);
                if (i < Count - 1)
                    Console.Write(", ");
            }
            Console.WriteLine("");
        }


        /*
         * helper functions
         */

        /// <summary>
        /// Determines whether there are multiple Nodes in the PQueue with the same lowest Fscore.
        /// </summary>
        private void recomputeStats()
        {
            this.Count = pq.Count;
            if (pq.Count < 2)
                hasLeadTie = false;
            else if (pq[0].Fscore == pq[1].Fscore)
                hasLeadTie = true;
            else
                hasLeadTie = false;
        }


	}
}
