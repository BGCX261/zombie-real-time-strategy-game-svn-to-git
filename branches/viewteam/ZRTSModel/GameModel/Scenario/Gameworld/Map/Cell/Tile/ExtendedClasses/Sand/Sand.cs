using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class Sand : Tile
    {
        override public void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is SandVisitor)
            {
                SandVisitor sandVisitor = (SandVisitor)visitor;
                sandVisitor.Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }
    }
}
