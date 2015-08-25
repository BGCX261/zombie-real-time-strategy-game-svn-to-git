using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ZRTSModel
{
    public class ScenarioXMLWriter : ModelComponentVisitor
    {
        private XmlWriter output;

        private ScenarioXMLWriter()
        { }

        public ScenarioXMLWriter(Stream stream)
        {
            output = XmlWriter.Create(stream);
        }

        public void BeginWrite()
        {
            output.WriteStartDocument();
        }

        public void EndWrite()
        {
            output.WriteEndDocument();
            output.Flush();
        }

        public void Visit(Sand sand)
        {
            output.WriteStartElement("Sand");
            VisitChildren(sand);
            output.WriteEndElement();
        }

        public void Visit(Mountain mountain)
        {
            output.WriteStartElement("Mountain");
            VisitChildren(mountain);
            output.WriteEndElement();
        }

        public void Visit(Grass grass)
        {
            output.WriteStartElement("Grass");
            VisitChildren(grass);
            output.WriteEndElement();
        }

        public void Visit(MapGold gold)
        {
            output.WriteStartElement("Gold");
            VisitChildren(gold);
            output.WriteEndElement();
        }

        public void Visit(MapMetal metal)
        {
            output.WriteStartElement("Metal");
            VisitChildren(metal);
            output.WriteEndElement();
        }

        public void Visit(MapWood wood)
        {
            output.WriteStartElement("Wood");
            VisitChildren(wood);
            output.WriteEndElement();
        }

        public void Visit(MapResource mapResource)
        {
            output.WriteStartElement("MapResource");
            VisitChildren(mapResource);
            output.WriteEndElement();
        }

        public void Visit(CellComponent cell)
        {
            output.WriteStartElement("Cell");
            output.WriteAttributeString("X", cell.X.ToString());
            output.WriteAttributeString("Y", cell.Y.ToString());
            VisitChildren(cell);
            output.WriteEndElement();
        }

        public void Visit(Map map)
        {
            output.WriteStartElement("Map");
            output.WriteAttributeString("Width", map.GetWidth().ToString());
            output.WriteAttributeString("Height", map.GetHeight().ToString());
            VisitChildren(map);
            output.WriteEndElement();
        }

        public void Visit(Gameworld gameworld)
        {
            output.WriteStartElement("Gameworld");
            VisitChildren(gameworld);
            output.WriteEndElement();
        }

        public void Visit(ScenarioComponent scenario)
        {
            output.WriteStartElement("Scenario");
            VisitChildren(scenario);
            output.WriteEndElement();
        }

        public void Visit(PlayerList list)
        {
            output.WriteStartElement("PlayerList");
            VisitChildren(list);
            output.WriteEndElement();
        }

        public void Visit(PlayerComponent player)
        {
            output.WriteStartElement("Player");
            output.WriteAttributeString("Name", player.Name);
            output.WriteAttributeString("Race", player.Race);
            output.WriteAttributeString("Gold", player.Gold.ToString());
            output.WriteAttributeString("Metal", player.Metal.ToString());
            output.WriteAttributeString("Wood", player.Wood.ToString());
            VisitChildren(player);
            output.WriteEndElement();
        }

        public void Visit(BuildingList list)
        {
            output.WriteStartElement("BuildingList");
            VisitChildren(list);
            output.WriteEndElement();
        }

        public void Visit(PlayerResources resources)
        {
            // Do nothing
        }

        public void Visit(UnitList list)
        {
            output.WriteStartElement("UnitList");
            VisitChildren(list);
            output.WriteEndElement();
        }

        public void Visit(UnitComponent unit)
        {
            output.WriteStartElement("Unit");
            output.WriteAttributeString("Type", unit.Type);
            output.WriteAttributeString("CanAttack", unit.CanAttack.ToString());
            output.WriteAttributeString("Attack", unit.Attack.ToString());
            output.WriteAttributeString("AttackRange", unit.AttackRange.ToString());
            output.WriteAttributeString("AttackTicks", unit.AttackTicks.ToString());
            output.WriteAttributeString("BuildSpeed", unit.BuildSpeed.ToString());
            output.WriteAttributeString("CanBuild", unit.CanBuild.ToString());
            output.WriteAttributeString("CanHarvest", unit.CanHarvest.ToString());
            output.WriteAttributeString("CurrentHealth", unit.CurrentHealth.ToString());
            output.WriteAttributeString("MaxHealth", unit.MaxHealth.ToString());
            output.WriteAttributeString("X", unit.PointLocation.X.ToString());
            output.WriteAttributeString("Y", unit.PointLocation.Y.ToString());
            output.WriteAttributeString("Speed", unit.Speed.ToString());
            VisitChildren(unit);
            output.WriteEndElement();
        }

        public void Visit(ActionQueue queue)
        {
            // We do not output the action queue, or any of its children.
        }

        public void Visit(ModelComponent component)
        {
            // Do nothing.
        }

        private void VisitChildren(ModelComponent component)
        {
            foreach (ModelComponent child in component.GetChildren())
            {
                child.Accept(this);
            }
        }


        public void Visit(Building building)
        {
            output.WriteStartElement("Building");
            output.WriteAttributeString("Type", building.Type);
            output.WriteAttributeString("CurrentHealth", building.CurrentHealth.ToString());
            output.WriteAttributeString("MaxHealth", building.MaxHealth.ToString());
            output.WriteAttributeString("X", building.PointLocation.X.ToString());
            output.WriteAttributeString("Y", building.PointLocation.Y.ToString());
            output.WriteAttributeString("CanProduce", building.CanProduce.ToString());
            output.WriteAttributeString("Completed", building.Completed.ToString());
            output.WriteAttributeString("DropOffResources", building.DropOffResources.ToString());
            output.WriteAttributeString("FoodCost", building.FoodCost.ToString());
            output.WriteAttributeString("Height", building.Height.ToString());
            output.WriteAttributeString("Width", building.Width.ToString());
            output.WriteAttributeString("LumberCost", building.LumberCost.ToString());
            output.WriteAttributeString("MetalCost", building.MetalCost.ToString());
            output.WriteAttributeString("WaterCost", building.WaterCost.ToString());
            foreach (string s in building.ProductionTypes)
            {
                output.WriteAttributeString("ProductionType", s);
            }
            VisitChildren(building);
            output.WriteEndElement();
        }
    }
}
