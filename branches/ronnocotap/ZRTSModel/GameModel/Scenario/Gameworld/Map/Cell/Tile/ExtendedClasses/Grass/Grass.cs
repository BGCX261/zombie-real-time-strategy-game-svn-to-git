using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class Grass : Tile
    {
        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is GrassVisitor)
            {
                GrassVisitor grassVisitor = (GrassVisitor)visitor;
                grassVisitor.Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
