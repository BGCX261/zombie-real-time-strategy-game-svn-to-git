using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    /// <summary>
    /// A queue of actions to be taken over time.
    /// </summary>
    [Serializable()]
    public class ActionQueue : ModelComponent
    {
		/// <summary>
		/// Performs whatever current action is on the queue. If the action is complete, it will remove the action and set the 
		/// State of the UnitComponent containing this ActionQueue to IDLE.
		/// </summary>
        public void Work()
        {
            if (GetChildren().Count != 0)
            {
                EntityAction action = GetChildren()[0] as EntityAction;
				if (action.Work())
				{
					RemoveChild(action);
					((UnitComponent)Parent).State = UnitComponent.UnitState.IDLE;
				}
            }
            
        }
        public override void AddChild(ModelComponent child)
        {
            if (child is EntityAction)
                base.AddChild(child);
        }
        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
