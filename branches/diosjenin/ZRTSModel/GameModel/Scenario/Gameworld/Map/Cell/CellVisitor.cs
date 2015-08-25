using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface CellVisitor
    {
        void Visit(CellComponent cell);
    }
}
