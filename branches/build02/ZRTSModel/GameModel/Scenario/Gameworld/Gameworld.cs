using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    /// <summary>
    /// Represents the physical game state, not logic (triggers).  Contains a map and a player list.
    /// </summary>
    [Serializable()]
    public class Gameworld : ModelComponent
    {
        public Gameworld()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Width</param>
        /// <param name="y">Height</param>
        public Gameworld(int x, int y)
        {
            AddChild(new Map(x, y));
            AddChild(new PlayerList());
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



        public PlayerList GetPlayerList()
        {
            foreach (ModelComponent component in GetChildren())
            {
                if (component is PlayerList)
                {
                    return (PlayerList)component;
                }
            }
            return null;
        }

        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
