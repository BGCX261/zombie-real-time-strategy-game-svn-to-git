using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class Gameworld : ModelComponent
    {

        public Gameworld(int x, int y)
        {
            AddChild(new Map(x, y));
        }
        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is GameworldVisitor)
            {
                ((GameworldVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }

        public Map GetMap()
        {
            foreach (ModelComponent component in GetChildren())
            {
                if (component is Map)
                {
                    return (Map)component;
                }
            }
            return null;
        }
    }
}
