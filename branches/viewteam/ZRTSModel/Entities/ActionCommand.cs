using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    /// <summary>
    /// This interface will be used to define all commands that can be given to Entities.
    /// </summary>
    [Serializable()]
    public class ActionCommand
    {
        /// <summary>
        /// A public enum to identify what kind of action each class that implements ActionCommand is. Every class that
        /// implements ActionCommand must have a corresponding value in this enum.
        /// </summary>
        public enum ActionType { Move, SimpleAttack, BuildBuilding, BuildUnit, Harvest };
        public ActionType actionType;
        /// <summary>
        /// Perform the action.
        /// </summary>
        /// <returns>true if the action is completed, false if the action is not completed.</returns>
        public virtual bool work()
        {
            return true;
        }
    }
}
