using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSLogic.Event
{
    public class MoveEventDelegator : EventDelegator
    {
        override public void Visit(MoveEvent moveEvent)
        {
            NotifyAll(moveEvent);
        }
    }
}
