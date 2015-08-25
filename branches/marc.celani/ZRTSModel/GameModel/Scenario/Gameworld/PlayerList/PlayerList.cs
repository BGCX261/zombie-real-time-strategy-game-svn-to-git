using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.EventHandlers;

namespace ZRTSModel
{
    /// <summary>
    /// A list of players - can only contain players.
    /// </summary>
    [Serializable()]
    public class PlayerList : ModelComponent
    {
        public event PlayerListChangedHandler PlayerAddedEvent;
        public event PlayerListChangedHandler PlayerRemovedEvent;

        /// <summary>
        /// Adds a PlayerComponent to the list
        /// </summary>
        /// <param name="child"></param>
        public override void AddChild(ModelComponent child)
        {
            if (child is PlayerComponent)
            {
                base.AddChild(child);
                PlayerListChangedEventArgs e = new PlayerListChangedEventArgs();
                e.PlayersAddedOrRemoved.Add((PlayerComponent)child);
                if (PlayerAddedEvent != null)
                {
                    PlayerAddedEvent(this, e);
                }
            }
        }

        /// <summary>
        /// Removes a PlayerComponent from the list
        /// </summary>
        /// <param name="child"></param>
        public override void RemoveChild(ModelComponent child)
        {
            base.RemoveChild(child);
            PlayerListChangedEventArgs e = new PlayerListChangedEventArgs();
            e.PlayersAddedOrRemoved.Add((PlayerComponent)child);
            if (PlayerRemovedEvent != null)
            {
                PlayerRemovedEvent(this, e);
            }
        }

        /// <summary>
        /// Implements Visitor pattern
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(ModelComponentVisitor visitor)
        {
            visitor.Visit(this);
        }

        public void FireChangedEvent()
        {
            if (PlayerAddedEvent != null)
            {
                PlayerAddedEvent(this, null);
            }
        }

        /// <summary>
        /// Retrieve a player by its name
        /// </summary>
        /// <param name="p">Player's name</param>
        /// <returns>Requested PlayerComponent</returns>
        public PlayerComponent GetPlayerByName(string p)
        {
            PlayerComponent player = null;
            foreach (PlayerComponent pc in GetChildren())
            {
                if (pc.Name.Equals(p))
                {
                    player = pc;
                    break;
                }
            }
            return player;
        }
    }
}
