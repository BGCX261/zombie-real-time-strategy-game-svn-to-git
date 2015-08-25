using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    /// <summary>
    /// An abstract class representing a particular resource which may be placed in a cell.
    /// </summary>
    [Serializable()]
    public abstract class MapResource : ModelComponentLeaf
    {
        protected int amountRemaining;
        
        // Require to be constructed with amount remaining.
        private MapResource()
        {

        }

        public MapResource(int amount)
        {
            amountRemaining = amount;
        }

        public int getAmountRemaining()
        {
            return amountRemaining;
        }

        public void setAmountRemaining(int amount)
        {
            amountRemaining = amount;
        }
    }
}
