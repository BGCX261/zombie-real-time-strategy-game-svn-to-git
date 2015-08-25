using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZRTSModel.GameWorld;

namespace ZRTSModel.Entities
{
    /// <summary>
    /// Class declaration for Static Entities (Objects, Resources, and Buildings)
    /// </summary>
    [Serializable()]
    public class StaticEntity : Entity
    {

        public Cell orginCell;
        public byte width, height;

        public StaticEntity(Player.Player owner, short health, byte width, byte height) : base(owner, health)
        {
            this.width = width;
            this.height = height;
        }

        public void setOrginCell(Cell orginCell)
        {
            this.orginCell = orginCell;
        }
    }
}
