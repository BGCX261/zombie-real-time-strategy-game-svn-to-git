using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class ActionQueue : ModelComponent
    {
        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is ActionQueueVisitor)
            {
                ((ActionQueueVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
