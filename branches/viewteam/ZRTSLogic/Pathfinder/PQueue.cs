using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

namespace Pathfinder
{
    /// <summary>
    /// A custom-made priority queue for Cells; ordered by ascending Fscore to promote efficient pathfinding.
    /// </summary>
	class PQueue
	{
		/*
		 * private variables
		 */

		private List<Cell> pq;


		/*
		 * constructors
		 */

        /// <summary>
        /// Blank constructor
        /// </summary>
		public PQueue()
		{
			pq = new List<Cell>();
		}

        /// <summary>
        /// Copy constructor
        /// </summary>
        public PQueue(PQueue p)
        {
            for (int i = 0; i < p.pq.Count(); i++)
                pq.Add(p.pq[i]);
        }


        /*
         * public functions
         */

        /// <summary>
        /// Removes the first Cell in the PQueue and returns it
        /// </summary>
        /// <returns>The first Cell in the PQueue</returns>
        public Cell dequeue()
		{
			Cell temp = pq[0];
			pq.RemoveAt(0);
			return temp;
		}

        /// <summary>
        /// Inserts the given Cell by its Fscore
        /// </summary>
        /// <param name="cell">The Cell to insert</param>
		public void enqueue(Cell cell)
		{
			if (pq.Count == 0)											// if the List is currently empty, add the Cell immediately
				pq.Add(cell);
			else
			{
                int i = 0;
                while (i < pq.Count)
                {
                    if (cell.Fscore >= pq[i].Fscore)
                        i++;
                    else
                        break;
                }
                pq.Insert(i, cell);
                /*
				int start = 0;
				int end = pq.Count - 1;
				int i = 0;
				while (start < end)
				{
					i = (end + start) / 2;								// otherwise, since the List is already sorted, do a binary search by Fscore
					if (cell.Fscore == pq[i].Fscore)					// to find the appropriate insertion index.
						break;											// note that we break on the first match, even if there are multiple matches;
					else if (cell.Fscore < pq[i].Fscore)				// the next loop fixes ties.
						end = i;
					else
						start = i + 1;
				}
				while (i < pq.Count && cell.Fscore == pq[i].Fscore)		// cells with matching Fscores are placed in FIFO order (preference is given to Cells found earlier);
					i++;												// if the index we found in the previous loop holds a Cell with a matching Fscore,
				pq.Insert(i, cell);										// increment the index until we find a Cell with a higher Fscore or reach the end of the List.
                 * */
			}
		}

        /// <summary>
        /// Returns the number of Cells in the PQueue
        /// </summary>
        /// <returns>The number of Cells in the PQueue</returns>
		public int count()
		{
			return pq.Count;
		}

        /// <summary>
        /// Returns true if the PQueue contains the given Cell, false otherwise
        /// </summary>
        /// <param name="cell">The Cell to check for</param>
        /// <returns>True if the PQueue contains the given Cell, false otherwise</returns>
		public bool contains(Cell cell)
		{
			return pq.Contains(cell);
		}

        /// <summary>
        /// A debugging function; prints the PQueue's contents
        /// </summary>
        /// <param name="name">The string name of the list to print</param>
        /// <param name="includeF">Toggle for printing the FScore of each Cell after its coordinates</param>
        public void print(string name, bool includeF)
        {
            Console.Write("  {0}: ", name);
            for (int i = 0; i < pq.Count; i++)
            {
                Console.Write("({0}, {1})", pq[i].Xcoord, pq[i].Ycoord);
                if (includeF)
                    Console.Write("+{0}", pq[i].Fscore);
                if (i < pq.Count - 1)
                    Console.Write(", ");
            }
            Console.WriteLine("");
        }

	}
}
