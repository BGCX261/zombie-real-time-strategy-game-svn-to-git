using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    /// <summary>
    /// A model component without children.
    /// </summary>
    [Serializable()]
    public abstract class ModelComponentLeaf : ModelComponent
    {
        public virtual void AddChild(ModelComponent child)
        {
            // No op
        }

        public virtual void RemoveChild(ModelComponent child)
        {
            // No op
        }
    }
}
