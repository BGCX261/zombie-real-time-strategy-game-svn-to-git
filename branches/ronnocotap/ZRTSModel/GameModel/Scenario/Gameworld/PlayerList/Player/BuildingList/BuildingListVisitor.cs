using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface BuildingListVisitor
    {
        void Visit(BuildingList list);
    }
}
