using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class MapGold : MapResource
    {

        public MapGold(int amount)
            : base(amount)
        {

        }

        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is MapGoldVisitor)
            {
                ((MapGoldVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
