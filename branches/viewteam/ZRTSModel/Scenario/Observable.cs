using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Scenario
{
    /// <summary>
    /// Observable objects
    /// This class will act as a subject to be observed by the observer, which can be from view object.
    /// 
    /// Any object that needs to be observed, need to be a subclass of this class.
    /// 
    /// Observer pattern
    /// </summary>
    [Serializable()]
    public class Observable
    {
        protected List<ZRTSModel.Scenario.Observer> observersList;       // List of observers (container)

        /* Game specific observer */
        protected ZRTSModel.Scenario.ViewSelectObserver viewSelectObserver;       // View select observer


        /// <summary>
        /// Default Constructor
        /// </summary>
        public Observable()
        {
            this.observersList = new List<ZRTSModel.Scenario.Observer>();
            viewSelectObserver = null;
        }


        /// <summary>
        /// Register a new observer
        /// </summary>
        /// <param name="obs">Observer object</param>
        public void register(ZRTSModel.Scenario.Observer obs)
        {
            this.observersList.Add(obs);

            System.Console.Out.WriteLine(obs.GetType().ToString());

            // Check if the observer is View observer
            if (obs.GetType().ToString().Equals("ZRTS.ViewSelect"))
            {
                viewSelectObserver = (ZRTSModel.Scenario.ViewSelectObserver) obs;
                System.Console.Out.WriteLine("Found!!!");
            }
        }

        /// <summary>
        /// Remove an observer from the list
        /// </summary>
        /// <param name="obs">Removed observer object</param>
        public void unregister(ZRTSModel.Scenario.Observer obs)
        {
            if (this.observersList.Contains(obs))
            {
                this.observersList.RemoveAt(this.observersList.IndexOf(obs));
            }
        }


        /// <summary>
        /// Notify all observers
        /// </summary>
        public void notify()
        {
            //System.Console.Out.WriteLine("Notify all obsevers!");
            foreach (ZRTSModel.Scenario.Observer obj in this.observersList)
            {
                obj.update();
            }
        }
    }
}
