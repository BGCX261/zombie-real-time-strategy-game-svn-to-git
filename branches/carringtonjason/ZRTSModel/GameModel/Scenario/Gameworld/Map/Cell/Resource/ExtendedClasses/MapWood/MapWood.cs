using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    class MapWood : MapResource
    {
        public MapWood(int amount)
            : base(amount)
        {

        }
        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is MapWoodVisitor)
            {
                ((MapWoodVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
