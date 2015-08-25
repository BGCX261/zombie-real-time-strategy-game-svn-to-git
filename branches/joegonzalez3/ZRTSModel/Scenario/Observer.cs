using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Scenario
{
    /// <summary>
    /// Generic observer object
    /// </summary>
    [Serializable()]
    public class Observer
    {
        public ZRTSModel.Scenario.ScenarioObservable observedScenario;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Observer()
        {

        }

        /// <summary>
        /// Notify this observer
        /// </summary>
        public virtual void notify()
        {
            // call derived method in the subclass
        }
    }
}
