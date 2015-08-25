using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    interface UnitVisitor
    {
        void Visit(UnitComponent unit);
    }
}
