using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface GameworldVisitor
    {
        void Visit(Gameworld gameworld);
    }
}
