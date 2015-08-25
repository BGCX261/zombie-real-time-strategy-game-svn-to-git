using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    [Serializable()]
    public class Building : StaticEntity
    {
        public Type type = Type.Building;

        public Building(Player.Player owner, short health, short maxHealth, byte width, byte height) 
            : base(owner, health, maxHealth, width, height)
        {

            this.entityType = EntityType.Building;
        }
    }
}
