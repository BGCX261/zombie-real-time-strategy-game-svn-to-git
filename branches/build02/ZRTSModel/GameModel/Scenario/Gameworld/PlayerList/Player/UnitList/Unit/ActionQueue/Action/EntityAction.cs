using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
	/// <summary>
	/// Abstract class to be extended by all actions that Units may perform.
	/// </summary>
    public abstract class EntityAction : ModelComponent
    {
        /// <summary>
        /// Works on the current action.
        /// </summary>
        /// <returns>true if the action is complete, false otherwise.</returns>
        public abstract bool Work();
        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
