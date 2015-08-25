using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    class CreateObserverListVisitor : ModelComponentVisitor
    {

        public void Visit(ModelComponent component)
        {
            component.UnregisterAll();
            foreach (ModelComponent c in component.GetChildren())
            {
                c.Accept(this);
            }
        }
    }
}
