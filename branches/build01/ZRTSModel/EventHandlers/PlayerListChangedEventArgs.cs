using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
    public class PlayerListChangedEventArgs : EventArgs
    {
        private List<PlayerComponent> playersAddedOrRemoved = new List<PlayerComponent>();

        public List<PlayerComponent> PlayersAddedOrRemoved
        {
            get { return playersAddedOrRemoved; }
            set { playersAddedOrRemoved = value; }
        }
    }
}
