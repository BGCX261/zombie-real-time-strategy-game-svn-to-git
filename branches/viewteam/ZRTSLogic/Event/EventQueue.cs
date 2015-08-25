using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSLogic.Event
{
    class EventQueue
    {
        
        private Queue<Event> queue = new Queue<Event>();
        private List<EventDelegator> delegators = new List<EventDelegator>();

        // Queueing Interface
        public void Enqueue(Event e)
        {
            queue.Enqueue(e);
        }

        public Event Dequeue()
        {
            return queue.Dequeue();
        }

        public Event Peek()
        {
            return queue.Peek();
        }

        // Observable Interface
        public void Register(EventDelegator delegator)
        {
            delegators.Add(delegator);
        }

        public void Unregister(EventDelegator delegator)
        {
            delegators.Remove(delegator);
        }

        // Process Events Interface
        public void Process()
        {
            if (queue.Count != 0)
            {
                Event e = Dequeue();
                foreach (EventDelegator delegator in delegators)
                {
                    e.Accept(delegator);
                }
            }
        }

    }
}
