using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZRTSModel.GameWorld;

namespace Pathfinder
{
	
    /// <summary>
    /// Contains advanced pathfinding functions.
    /// </summary>
	class Advanced
	{
		/*
		 * public functions
		 */

        /// <summary>
        /// Given an invalid Node, returns the approximate nearest valid Node.
        /// </summary>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Node nearestValidEnd(NodeMap map, Node start, Node end)
        {
            int max = Math.Max(map.height, map.width);
            PQueue ring = new PQueue();

            // iterate outward from the end Node in progressively larger rings, putting all valid Nodes into a PQueue.
            // Continue until we find a ring with a valid Node.
            for (int i = 1; i < max; i++)
            {
                ring = getRing(map, end, i);
                if (ring.Count != 0)
                    break;
            }

            // find the valid Node nearest the intended end
            Node newEnd = ring.peek(0);

            // if the PQueue has a lead tie (i.e. there are 2+ nodes of equal distance from the center node),
            // pick the one closest to the intended start Node.
            if (ring.hasLeadTie)
            {
                int distToStart = map.pathDistance(start, newEnd);
                int distToEnd = map.pathDistance(end, newEnd);
                for (int i = 1; i < ring.Count; i++)
                {
                    if (map.pathDistance(ring.peek(i), end) > distToEnd)
                        break;
                    int currentDist = map.pathDistance(ring.peek(i), start);
                    if (currentDist < distToStart)
                    {
                        distToStart = currentDist;
                        newEnd = ring.peek(i);
                    }
                }
            }
            
            // clean all enqueued Nodes so they can be properly used by the pathfinder
			for (int i = 0; i < ring.Count; i++)
				ring.peek(i).clean();

            return newEnd;
        }

		
		/*
		 * helper functions
		 */

        /// <summary>
        /// Enqueues all valid Nodes in a ring around a center Node, offset from the center by [offset] Nodes.
        /// For example, an offset of 2 searches a 5x5 ring (x+-2, y+-2) around the center Node.
        /// Since modifying the Node's Fscores is necessary for enqueueing, each enqueued node's Fscore must be reset after use.
        /// </summary>
        /// <param name="map">The NodeMap to search over</param>
        /// <param name="center">The center Node to search around</param>
        /// <param name="offset">The ring "radius"</param>
        /// <returns>A PQueue containing all valid Nodes found in the ring</returns>
        private static PQueue getRing(NodeMap map, Node center, int offset)
        {
            PQueue ring = new PQueue();
            int x = center.X;
            int y = center.Y;

            // grab left and right columns
            for (int i = Math.Max(y - offset, 0); i <= y + offset; i++)
            {
                Node Xmin = map.getNode(x - offset, i);
                Node Xmax = map.getNode(x + offset, i);
                if (Xmin != null && Xmin.isValid)
                {
                    Xmin.Fscore = map.pathDistance(Xmin, center);
                    ring.enqueue(Xmin);
                }
                if (Xmax != null && Xmax.isValid)
                {
                    Xmax.Fscore = map.pathDistance(Xmax, center);
                    ring.enqueue(Xmax);
                }
            }

            // grab remainder of top and bottom rows
            for (int i = x - offset + 1; i < x + offset; i++)
            {
                Node Ymin = map.getNode(i, Math.Max(0, y - offset));
                Node Ymax = map.getNode(i, Math.Min(map.width -1, y + offset));
                if (Ymin != null && Ymin.isValid)
                {
                    Ymin.Fscore = map.pathDistance(Ymin, center);
                    ring.enqueue(Ymin);
                }
                if (Ymax != null && Ymax.isValid)
                {
                    Ymax.Fscore = map.pathDistance(Ymax, center);
                    ring.enqueue(Ymax);
                }
            }

            return ring;
        }


	}
}
