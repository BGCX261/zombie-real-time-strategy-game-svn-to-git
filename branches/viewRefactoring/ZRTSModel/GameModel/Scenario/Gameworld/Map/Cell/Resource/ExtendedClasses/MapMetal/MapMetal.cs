using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    class MapMetal : MapResource
    {

        public MapMetal(int amount)
            : base(amount)
        {

        }

        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is MapMetalVisitor)
            {
                ((MapMetalVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
