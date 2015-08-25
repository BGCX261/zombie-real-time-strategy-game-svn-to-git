using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    interface MapResourceVisitor : ModelComponentVisitor
    {
        void Visit(MapResource mapResource);
    }
}
