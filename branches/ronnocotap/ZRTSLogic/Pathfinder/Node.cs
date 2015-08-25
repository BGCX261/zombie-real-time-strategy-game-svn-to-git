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

        public byte Xcoord;
        public byte Ycoord;
        public bool isValid;

        public int Fscore = 0;
        public int Gscore = 0;
        public int Hscore = 0;
        public Node prev = null;


        /*
         * constructors
         */

        public Node(byte x, byte y, bool valid)
        {
            this.Xcoord = x;
            this.Ycoord = y;
            this.isValid = valid;
        }


        /*
         * public functions
         */




        /*
         * helper functions
         */


    }
}
