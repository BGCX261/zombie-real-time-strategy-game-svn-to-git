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
        public Type type;

        public StaticEntity(Player.Player owner, short health, short maxHealth, byte width, byte height) : base(owner, health, maxHealth)
        {
            this.width = width;
            this.height = height;
        }

        public void setOrginCell(Cell orginCell)
        {
            this.orginCell = orginCell;
        }

        public enum Type
        {
            Object, Resource, Building
        }
    }
}
