using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using ZRTSModel.GameModel;

namespace ZRTSModel
{
    public class ScenarioXMLReader
    {
        private XmlReader reader;

        private ScenarioXMLReader()
        { }

        public ScenarioXMLReader(Stream stream)
        {
            reader = XmlReader.Create(stream);
        }

        public ScenarioComponent GenerateScenarioFromXML()
        {
            ScenarioComponent scenario = null;
            if (reader.Read())
            {
                // Go to the Scenario (skip the XML line)
                reader.Read();
                scenario = new ScenarioComponent();
                ModelComponent currentComponent = scenario;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            switch (reader.Name)
                            {
                                case "Gameworld":
                                    Gameworld gameworld = new Gameworld();
                                    currentComponent.AddChild(gameworld);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = gameworld;
                                    break;
                                case "Map":
                                    int width = Int32.Parse(reader.GetAttribute("Width"));
                                    int height = Int32.Parse(reader.GetAttribute("Height"));
                                    Map map = new Map(width, height);
                                    currentComponent.AddChild(map);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = map;
                                    break;
                                case "Cell":
                                    int x = Int32.Parse(reader.GetAttribute("X"));
                                    int y = Int32.Parse(reader.GetAttribute("Y"));
                                    CellComponent cell = new CellComponent();
                                    cell.X = x;
                                    cell.Y = y;
                                    currentComponent.AddChild(cell);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = cell;
                                    break;
                                case "PlayerList":
                                    PlayerList playerList = new PlayerList();
                                    currentComponent.AddChild(playerList);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = playerList;
                                    break;
                                case "Player":
                                    PlayerComponent player = new PlayerComponent();
                                    player.Name = reader.GetAttribute("Name");
                                    player.Race = reader.GetAttribute("Race");
                                    player.Gold = Int32.Parse(reader.GetAttribute("Gold"));
                                    player.Metal = Int32.Parse(reader.GetAttribute("Metal"));
                                    player.Wood = Int32.Parse(reader.GetAttribute("Wood"));
                                    currentComponent.AddChild(player);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = player;
                                    break;
                                case "BuildingList":
                                    if (!reader.IsEmptyElement)
                                        currentComponent = ((PlayerComponent)currentComponent).BuildingList;
                                    break;
                                case "UnitList":
                                    if (!reader.IsEmptyElement)
                                        currentComponent = ((PlayerComponent)currentComponent).GetUnitList();
                                    break;
                                case "Sand":
                                    Sand sand = new Sand();
                                    currentComponent.AddChild(sand);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = sand;
                                    break;
                                case "Mountain":
                                    Mountain mountain = new Mountain();
                                    currentComponent.AddChild(mountain);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = mountain;
                                    break;
                                case "Grass":
                                    Grass grass = new Grass();
                                    currentComponent.AddChild(grass);
                                    if (!reader.IsEmptyElement)
                                        currentComponent = grass;
                                    break;
                                case "Unit":
                                    UnitComponent unit = new UnitComponent();
                                    currentComponent.AddChild(unit);
                                    float unitX = float.Parse(reader.GetAttribute("X"));
                                    float unitY = float.Parse(reader.GetAttribute("Y"));
                                    unit.PointLocation = new PointF(unitX, unitY);
                                    unit.Type = reader.GetAttribute("Type");
                                    unit.MaxHealth = short.Parse(reader.GetAttribute("MaxHealth"));
                                    unit.CurrentHealth = short.Parse(reader.GetAttribute("CurrentHealth"));
                                    unit.CanHarvest = bool.Parse(reader.GetAttribute("CanHarvest"));
                                    unit.CanAttack = bool.Parse(reader.GetAttribute("CanAttack"));
                                    unit.Attack = short.Parse(reader.GetAttribute("Attack"));
                                    unit.AttackRange = float.Parse(reader.GetAttribute("AttackRange"));
                                    unit.AttackTicks = byte.Parse(reader.GetAttribute("AttackTicks"));
                                    unit.CanBuild = bool.Parse(reader.GetAttribute("CanBuild"));
                                    unit.BuildSpeed = byte.Parse(reader.GetAttribute("BuildSpeed"));
                                    unit.Speed = float.Parse(reader.GetAttribute("Speed"));
                                    /*
                                     * Type="zombie" CanAttack="True" 
                                     * Attack="20" AttackRange="4" AttackTicks="10" 
                                     * BuildSpeed="30" CanBuild="True" CanHarvest="False" 
                                     * CurrentHealth="100" MaxHealth="100" X="12" Y="13" 
                                     * Speed="0.1"
                                     */
                                    if (!reader.IsEmptyElement)
                                        currentComponent = unit;
                                    break;
                                case "Building":
                                    Building building = new Building();
                                    currentComponent.AddChild(building);
                                    building.Width = Int32.Parse(reader.GetAttribute("Width"));
                                    building.Height = Int32.Parse(reader.GetAttribute("Height"));
                                    building.PointLocation = new PointF(float.Parse(reader.GetAttribute("X")), float.Parse(reader.GetAttribute("Y")));
                                    building.Type = reader.GetAttribute("Type");
                                    building.CanProduce = bool.Parse(reader.GetAttribute("CanProduce"));
                                    if (!reader.IsEmptyElement)
                                        currentComponent = building;
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case XmlNodeType.EndElement:
                            if (currentComponent != null)
                                currentComponent = currentComponent.Parent;
                            break;
                    }

                }
                Console.WriteLine("XmlTextReader Properties Test");
                Console.WriteLine("===================");
                // Read this element's properties and display them on console
                Console.WriteLine("Name:" + reader.Name);
                Console.WriteLine("Base URI:" + reader.BaseURI);
                Console.WriteLine("Local Name:" + reader.LocalName);
                Console.WriteLine("Attribute Count:" + reader.AttributeCount.ToString());
                Console.WriteLine("Depth:" + reader.Depth.ToString());
                Console.WriteLine("Node Type:" + reader.NodeType.ToString());
                Console.WriteLine("Attribute Count:" + reader.Value.ToString());
            }
            return scenario;
        }
    }
}
