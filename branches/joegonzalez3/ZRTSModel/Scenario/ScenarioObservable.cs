using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Scenario
{
    /// <summary>
    /// Scenario Observable objects
    /// This class will act as a subject to be observed by the observer, which can be from view object.
    /// </summary>
    [Serializable()]
    public class ScenarioObservable
    {
        protected List<Observer> observersList;       // List of observers (container)

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScenarioObservable()
        {
            this.observersList = new List<Observer>();
        }


        /// <summary>
        /// Register a new observer
        /// </summary>
        /// <param name="obs">Observer object</param>
        public void register(Observer obs)
        {
            this.observersList.Add(obs);
            
        }

        /// <summary>
        /// Remove an observer from the list
        /// </summary>
        /// <param name="obs">Removed observer object</param>
        public void unregister(Observer obs)
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
            foreach(Observer obj in this.observersList)
            {
                obj.notify();
            }
        }

    }
}
