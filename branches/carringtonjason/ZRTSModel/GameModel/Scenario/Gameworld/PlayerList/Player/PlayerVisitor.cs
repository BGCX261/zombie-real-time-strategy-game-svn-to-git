using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface PlayerVisitor
    {
        void Visit(PlayerComponent player);
    }
}
