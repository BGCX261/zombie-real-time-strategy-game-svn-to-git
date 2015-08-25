using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class BuildingList : ModelComponent
    {
        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is BuildingListVisitor)
            {
                ((BuildingListVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
