using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.GameWorld
{
    [Serializable()]
    public class Tile
    {
        public string tileType;
        public bool passable;
        public int index;

        public Tile(string tileType, bool passable, int index)
        {
            this.tileType = tileType;
            this.passable = passable;
            this.index = index;
        }
    }
}
