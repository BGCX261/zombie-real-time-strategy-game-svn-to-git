using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public class NoOpModelComponentVisitor : ModelComponentVisitor
    {
        public virtual void Visit(Sand sand)
        {
            Visit((Tile)sand);
        }

        public virtual void Visit(Mountain mountain)
        {
            Visit((Tile)mountain);
        }

        public virtual void Visit(Grass grass)
        {
            Visit((Tile)grass);
        }

        public virtual void Visit(MapGold gold)
        {
            Visit((MapResource)gold);
        }

        public virtual void Visit(MapMetal metal)
        {
            Visit((MapResource)metal);
        }

        public virtual void Visit(MapWood wood)
        {
            Visit((MapResource)wood);
        }

        public virtual void Visit(MapResource mapResource)
        {
            Visit((ModelComponent)mapResource);
        }

        public virtual void Visit(CellComponent cell)
        {
            Visit((ModelComponent)cell);
        }

        public virtual void Visit(Map map)
        {
            Visit((ModelComponent)map);
        }

        public virtual void Visit(Gameworld gameworld)
        {
            Visit((ModelComponent)gameworld);
        }

        public virtual void Visit(ScenarioComponent scenario)
        {
            Visit((ModelComponent)scenario);
        }

        public virtual void Visit(PlayerList list)
        {
            Visit((ModelComponent)list);
        }

        public virtual void Visit(PlayerComponent player)
        {
            Visit((ModelComponent)player);
        }

        public virtual void Visit(BuildingList list)
        {
            Visit((ModelComponent)list);
        }

        public virtual void Visit(PlayerResources resources)
        {
            Visit((ModelComponent)resources);
        }

        public virtual void Visit(UnitList list)
        {
            Visit((ModelComponent)list);
        }

        public virtual void Visit(UnitComponent unit)
        {
            Visit((ModelComponent)unit);
        }

        public virtual void Visit(ActionQueue queue)
        {
            Visit((ModelComponent)queue);
        }

        public virtual void Visit(ModelComponent component)
        {
            // No op
        }


        public virtual void Visit(Building building)
        {
            Visit((ModelComponent)building);
        }
    }
}
