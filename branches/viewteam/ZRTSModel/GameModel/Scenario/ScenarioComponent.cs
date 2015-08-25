using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    [Serializable()]
    public class ScenarioComponent : ModelComponent
    {
        public ScenarioComponent(int x, int y)
        {
            AddChild(new Gameworld(x, y));
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            if (visitor is ScenarioVisitor)
            {
                ((ScenarioVisitor)visitor).Visit(this);
            }
            else
            {
                base.Accept(visitor);
            }
        }

        public Gameworld GetGameWorld()
        {
            foreach (ModelComponent component in GetChildren())
            {
                if (component is Gameworld)
                {
                    return (Gameworld)component;
                }
            }
            return null;
        }
    }
}
