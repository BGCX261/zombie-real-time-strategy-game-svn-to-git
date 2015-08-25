using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    /// <summary>
    /// A representation of the type of cell a cell is.
    /// </summary>
    [Serializable()]
    public abstract class Tile : ModelComponentLeaf
    {
        virtual public bool Passable()
        {
            return true;
        }
    }
}
