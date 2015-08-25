using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface ModelCommand
    {
        void Do();
        void Undo();
        bool CanBeDone();
    }
}
