using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZRTSModel
{
    public class StrategizedModelComponentVisitor : ModelComponentVisitor
    {
        public ModelComponentVisitor SandVisitor = null;
        public ModelComponentVisitor MountainVisitor = null;
        public ModelComponentVisitor GrassVisitor = null;
        public ModelComponentVisitor MapGoldVisitor = null;
        public ModelComponentVisitor MapMetalVisitor = null;
        public ModelComponentVisitor MapWoodVisitor = null;
        public ModelComponentVisitor MapResourceVisitor = null;
        public ModelComponentVisitor CellVisitor = null;
        public ModelComponentVisitor MapVisitor = null;
        public ModelComponentVisitor GameworldVisitor = null;
        public ModelComponentVisitor ScenarioVisitor = null;
        public ModelComponentVisitor PlayerListVisitor = null;
        public ModelComponentVisitor PlayerVisitor = null;
        public ModelComponentVisitor BuildingListVisitor = null;
        public ModelComponentVisitor PlayerResourcesVisitor = null;
        public ModelComponentVisitor UnitListVisitor = null;
        public ModelComponentVisitor UnitVisitor = null;
        public ModelComponentVisitor ActionQueueVisitor = null;
        public ModelComponentVisitor AbstractComponentVisitor = null;

        public void Visit(Sand sand)
        {
            if (SandVisitor != null)
                sand.Accept(SandVisitor);
        }

        public void Visit(Mountain mountain)
        {
            if (MountainVisitor != null)
                mountain.Accept(MountainVisitor);
        }

        public void Visit(Grass grass)
        {
            if (GrassVisitor != null)
                grass.Accept(GrassVisitor);
        }

        public void Visit(MapGold gold)
        {
            if (MapGoldVisitor != null)
                gold.Accept(MapGoldVisitor);
        }

        public void Visit(MapMetal metal)
        {
            if (MapMetalVisitor != null)
                metal.Accept(MapMetalVisitor);
        }

        public void Visit(MapWood wood)
        {
            if (MapWoodVisitor != null)
                wood.Accept(MapWoodVisitor);
        }

        public void Visit(MapResource mapResource)
        {
            if (MapResourceVisitor != null)
                mapResource.Accept(MapResourceVisitor);
        }

        public void Visit(CellComponent cell)
        {
            if (CellVisitor != null)
                cell.Accept(CellVisitor);
        }

        public void Visit(Map map)
        {
            if (MapVisitor != null)
                map.Accept(MapVisitor);
        }

        public void Visit(Gameworld gameworld)
        {
            if (GameworldVisitor != null)
                gameworld.Accept(GameworldVisitor);
        }

        public void Visit(ScenarioComponent scenario)
        {
            if (ScenarioVisitor != null)
                scenario.Accept(ScenarioVisitor);
        }

        public void Visit(PlayerList list)
        {
            if (PlayerListVisitor != null)
                list.Accept(PlayerListVisitor);
        }

        public void Visit(PlayerComponent player)
        {
            if (PlayerVisitor != null)
                player.Accept(PlayerVisitor);
        }

        public void Visit(BuildingList list)
        {
            if (BuildingListVisitor != null)
                list.Accept(BuildingListVisitor);
        }

        public void Visit(PlayerResources resources)
        {
            if (PlayerResourcesVisitor != null)
                resources.Accept(PlayerResourcesVisitor);
        }

        public void Visit(UnitList list)
        {
            if (UnitListVisitor != null)
                list.Accept(UnitListVisitor);
        }

        public void Visit(UnitComponent unit)
        {
            if (UnitVisitor != null)
                unit.Accept(UnitVisitor);
        }

        public void Visit(ActionQueue queue)
        {
            if (ActionQueueVisitor != null)
                queue.Accept(ActionQueueVisitor);
        }

        public void Visit(ModelComponent component)
        {
            if (AbstractComponentVisitor != null)
                component.Accept(AbstractComponentVisitor);
        }
    }
}
