using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ZRTSModel
{
    /// <summary>
    /// A component representing a particular scenario.  Contains a game world.
    /// </summary>
    [Serializable()]
    public class ScenarioComponent : ModelComponent
    {
		public List<Trigger.Trigger> triggers;
        public ScenarioComponent(int x, int y)
        {
            AddChild(new Gameworld(x, y));
			triggers = new List<Trigger.Trigger>();
        }

		public ScenarioComponent()
		{
			triggers = new List<Trigger.Trigger>();
		}

		public void addTrigger(Trigger.Trigger trigger)
		{
			triggers.Add(trigger);
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

        ~ScenarioComponent()
        {
            Debug.WriteLine("Scenario Component destructing.");
        }
        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
