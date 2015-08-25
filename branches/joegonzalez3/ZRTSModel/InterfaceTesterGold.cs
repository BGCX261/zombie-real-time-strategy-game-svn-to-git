using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public class InterfaceTesterGold : InterfaceTester, MapGoldVisitor
    {
        public void Visit(MapGold gold)
        {
            Console.WriteLine("Gold Visitor");
        }
    }
}
