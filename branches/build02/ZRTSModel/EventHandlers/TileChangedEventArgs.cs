using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.EventHandlers
{
    public class TileChangedEventArgs : EventArgs
    {
        private Tile tile;

        public Tile Tile
        {
            get { return tile; }
        }

        public TileChangedEventArgs(Tile tile)
        {
            this.tile = tile;
        }
    }
}
