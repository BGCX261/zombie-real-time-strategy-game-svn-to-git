using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.Entities;

namespace ZRTSModel
{
    [Serializable()]
    public class UnitList : ModelComponent
    {
        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is UnitListVisitor)
            {
                ((UnitListVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
