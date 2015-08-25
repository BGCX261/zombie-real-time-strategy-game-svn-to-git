using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    [Serializable()]
    class ObjectEntity : StaticEntity
    {
        public Type type = Type.Object;

        public ObjectEntity(Player.Player owner, short health, short maxHealth, byte width, byte height)
            : base(owner, health, maxHealth, width, height)
        {
            this.entityType = EntityType.Object;
        }
    }
}
