using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Scenario
{
    /// <summary>
    /// Observer Objects
    /// Generic observer object
    /// 
    /// </summary>
   // [Serializable()]
    public interface Observer
    {
        /// <summary>
        /// Notify this observer
        /// </summary>
       void update();
    }
}
