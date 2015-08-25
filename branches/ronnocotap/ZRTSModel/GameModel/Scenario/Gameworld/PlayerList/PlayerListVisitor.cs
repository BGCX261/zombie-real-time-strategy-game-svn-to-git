using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface PlayerListVisitor
    {
        void Visit(PlayerList list);
    }
}
