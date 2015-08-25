using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pathfinder
{
    /// <summary>
    /// A snap of a single Cell on the Map.  May upgrade to a QuadTree variant later.
    /// </summary>
    class Node
    {
		/*
		 * attributes
		 */

		// private data
        public int X;
        public int Y;
        public bool isValid;

		// pathfinder data
        public int Fscore = 0;
        public int Gscore = 0;
        public int Hscore = 0;
        public Node prev;

		// state data (open, closed, or unexamined)
		private enum state { Unexamined, Open, Closed };
		private state State { get; set; }
		public void open() { State = state.Open; }
		public bool isOpen { get { return (State == state.Open); } }
		public void close() { State = state.Closed; }
		public bool isClosed { get { return (State == state.Closed); } }


		/*
		 * constructors
		 */

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="x">The Node's X-coordinate</param>
		/// <param name="y">The Node's Y-coordinate</param>
		/// <param name="valid">True if the Node is valid for traversal, false otherwise</param>
        public Node(int x, int y, bool valid)
        {
            this.X = x;
            this.Y = y;
            this.isValid = valid;
			this.State = state.Unexamined;
        }


		/*
		 * public functions
		 */

		/// <summary>
		/// This function cleans a Node used during an advanced pathfinding operation so that it can be properly used by A*
		/// </summary>
		public void clean()
		{
			this.Fscore = 0;
			this.State = state.Unexamined;
		}

    }
}
