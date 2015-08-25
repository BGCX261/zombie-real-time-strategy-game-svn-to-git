using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public class InterfaceTester : ModelComponentVisitor
    {

        public void Visit(ModelComponent component)
        {
            Console.WriteLine("ModelComponentVisitor");
        }
    }
}
