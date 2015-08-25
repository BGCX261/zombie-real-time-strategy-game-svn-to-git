using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public interface ModelComponentVisitor
    {
        void Visit(Sand sand);
        void Visit(Mountain mountain);
        void Visit(Grass grass);
        void Visit(MapGold gold);
        void Visit(MapMetal metal);
        void Visit(MapWood wood);
        void Visit(MapResource mapResource);
        void Visit(CellComponent cell);
        void Visit(Map map);
        void Visit(Gameworld gameworld);
        void Visit(ScenarioComponent scenario);
        void Visit(PlayerList list);
        void Visit(PlayerComponent player);
        void Visit(BuildingList list);
        void Visit(PlayerResources resources);
        void Visit(UnitList list);
        void Visit(UnitComponent unit);
        void Visit(ActionQueue queue);
        void Visit(ModelComponent component);
        void Visit(Building building);
    }
}
