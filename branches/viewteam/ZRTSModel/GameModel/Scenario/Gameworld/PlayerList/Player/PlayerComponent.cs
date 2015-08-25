using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class PlayerComponent : ModelComponent
    {
        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is PlayerVisitor)
            {
                ((PlayerVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
