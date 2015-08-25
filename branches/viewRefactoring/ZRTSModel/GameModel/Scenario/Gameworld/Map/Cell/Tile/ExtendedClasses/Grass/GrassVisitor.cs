using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    interface GrassVisitor
    {
        void Visit(Grass grass);
    }
}
