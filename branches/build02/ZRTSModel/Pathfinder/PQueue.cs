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
		public int Count { get { return pq.Count; } }
		public bool hasLeadTie { get { return computeTie(); } }


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
			node.open();
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

        /*
         * helper functions
         */

        /// <summary>
        /// Determines whether there are multiple Nodes in the PQueue with the same lowest Fscore.
        /// </summary>
        private bool computeTie()
        {
            if (Count < 2)
                return false;
            else if (pq[0].Fscore == pq[1].Fscore)
                return true;
            return false;
        }


	}
}
