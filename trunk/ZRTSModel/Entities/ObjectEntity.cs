using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    [Serializable()]
    class ObjectEntity : StaticEntity
    {
        public ObjectEntity(Player.Player owner, short health, byte width, byte height)
            : base(owner, health, width, height)
        {
            this.entityType = EntityType.Object;
        }
    }
}
