using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSLogic.Event
{
    public interface Event
    {
        // Visitor pattern
        void Accept(EventHandler handler);
    }
}
