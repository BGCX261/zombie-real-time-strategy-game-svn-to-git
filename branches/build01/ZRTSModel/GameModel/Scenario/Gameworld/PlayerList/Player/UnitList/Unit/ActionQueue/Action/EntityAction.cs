using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public abstract class EntityAction : ModelComponent
    {
        /// <summary>
        /// Works on the current action.
        /// </summary>
        /// <returns>true if the work is complete</returns>
        public abstract bool Work();
        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
