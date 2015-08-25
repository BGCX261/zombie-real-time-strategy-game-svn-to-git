using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSLogic.Event
{
    public abstract class EventDelegator : NoOpEventHandler
    {
        private List<EventHandler> handlers = new List<EventHandler>();

        public void RegisterEventHandler(EventHandler handler)
        {
            handlers.Add(handler);
        }

        public void UnregisterEventHandler(EventHandler handler)
        {
            handlers.Remove(handler);
        }

        protected void NotifyAll(Event e)
        {
            foreach (EventHandler handler in handlers)
            {
                e.Accept(handler);
            }
        }
    }
}
