using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    [Serializable()]
    class ResourceEntity : StaticEntity
    {
        public enum ResourceType : byte { Water, Lumber, Food, Metal };
        ResourceType resourceType;
        public Type type = Type.Resource;

        public ResourceEntity(Player.Player owner, short health, short maxHealth, byte width, byte height, ResourceType type)
            : base(owner, health, maxHealth, width, height)
        {
            this.resourceType = type;
            this.entityType = EntityType.Resource;
        }
    }
}
