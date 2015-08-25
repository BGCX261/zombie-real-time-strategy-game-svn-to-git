using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    interface MapMetalVisitor : ModelComponentVisitor
    {
        void Visit(MapMetal metal);
    }
}
