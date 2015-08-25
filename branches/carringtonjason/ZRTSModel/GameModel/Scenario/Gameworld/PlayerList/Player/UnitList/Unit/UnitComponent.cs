using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    abstract class UnitComponent : ModelComponent
    {
        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is UnitVisitor)
            {
                ((UnitVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
