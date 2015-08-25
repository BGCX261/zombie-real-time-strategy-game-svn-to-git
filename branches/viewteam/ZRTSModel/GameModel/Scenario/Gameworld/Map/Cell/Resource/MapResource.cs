using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
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
            NotifyAll();
        }

        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is MapResourceVisitor)
            {
                ((MapResourceVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
