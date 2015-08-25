using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel.Entities
{
    [Serializable()]
    public class Building : StaticEntity
    {

        public BuildingStats stats;

        public bool isCompleted = false; // Has this building ever been completly built?

        public Building(Player.Player owner, short health, byte width, byte height) 
            : base(owner, health, width, height)
        {
            this.stats = new BuildingStats();
            this.entityType = EntityType.Building;
        }

        public Building(Player.Player owner, BuildingStats stats)
            : base(owner, stats.maxHealth, stats.width, stats.height)
        {
            this.stats = stats;
			this.entityType = EntityType.Building;
        }
    }
}
