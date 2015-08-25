using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface ModelComponentVisitor
    {

        void Visit(ModelComponent component);
    }
}
