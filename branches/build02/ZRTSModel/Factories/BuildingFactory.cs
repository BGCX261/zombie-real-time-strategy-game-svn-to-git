using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ZRTSModel;
using ZRTSModel.Entities;

namespace ZRTSModel.Factories
{
	/// <summary>
	/// This class will be used to create BuildingComponents based off of information stored in XML files 
	/// (under 'Content/buildings/'. Using the Singleton pattern, it will ensure that these XML files are only read in once.
	/// </summary>
    public class BuildingFactory
    {
		private static BuildingFactory instance;
        List<string> buildingTypes;
        Dictionary<string, BuildingStats> statsDict;

        string BASE_DIR = "Content/buildings/";
        string BLDG_LIST = "buildings.xml";

        private BuildingFactory()
        {
            buildingTypes = new List<string>();
            statsDict = new Dictionary<string, BuildingStats>();
            readXML();
        }

		public static BuildingFactory Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BuildingFactory();
				}
				return instance;
			}
		}

        private void readXML()
        {
            string listXML = readFile(BASE_DIR + BLDG_LIST);
            parseListXML(listXML);
        }

        private void parseListXML(string xml)
        {
            bool endOfList = false;
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            while (!endOfList)
            {
                try
                {
                    reader.ReadToFollowing("Building");
                    buildingTypes.Add(reader.ReadElementContentAsString());
                }
                catch
                {
                    endOfList = true;
                }
            }
            reader.Close();

            foreach (string s in buildingTypes)
            {
                BuildingStats stats = new BuildingStats();
                statsDict.Add(s, stats);

                string xmlBuilding = readFile(BASE_DIR + s + ".xml");

                readBuildingXML(xmlBuilding);
            }

            
        }

        private void readBuildingXML(string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            reader.ReadToFollowing("type");
            string type = reader.ReadElementContentAsString();

            reader.ReadToFollowing("width");
            byte width = (byte)reader.ReadElementContentAsInt();

            reader.ReadToFollowing("height");
            byte height = (byte)reader.ReadElementContentAsInt();

            reader.ReadToFollowing("maxHealth");
            short maxHealth = (short)reader.ReadElementContentAsInt();

            reader.ReadToFollowing("dropOffResources");
            bool dropOffResources = reader.ReadElementContentAsBoolean();

            reader.ReadToFollowing("canProduce");
            bool canProduce = reader.ReadElementContentAsBoolean();

            BuildingStats stats = statsDict[type];
            stats.buildingType = type;
            stats.width = width;
            stats.height = height;
            stats.maxHealth = maxHealth;
            stats.dropOffResources = dropOffResources;
            stats.canProduce = canProduce;
        }

        private string readFile(string fileName)
        {
            string input = "";
            try
            {
                StreamReader reader = new StreamReader(fileName);
                try
                {
                    do
                    {
                        input += (reader.ReadLine());
                    }
                    while (reader.Peek() != -1);
                }

                catch { }

                finally
                {
                    reader.Close();
                }
            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                throw new Exception(e.Message);
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception(e.Message);
            }


            return input;
        }

		/// <summary>
		/// Returns a List of strings where each string represents a type of Building.
		/// </summary>
		/// <returns></returns>
        public List<string> getBuildingTypes()
        {
            return this.buildingTypes;
        }

		/// <summary>
		/// Returns a dictionary that, when given a string that represents a type of Building, retrieves the BuildingStats
		/// object 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
        public BuildingStats getStats(string type)
        {
            return statsDict[type];
        }

        public Building Build(string buildingType, bool completed)
        {
            BuildingStats stats = getStats(buildingType);
            Building building = new Building();
            building.Type = buildingType;
            building.CanProduce = stats.canProduce;
            building.MaxHealth = stats.maxHealth;
            building.Completed = completed;
            building.CurrentHealth = completed ? stats.maxHealth : 0;
            building.DropOffResources = stats.dropOffResources;
            building.FoodCost = stats.foodCost;
            building.Height = stats.height;
            building.LumberCost = stats.lumberCost;
            building.MetalCost = stats.metalCost;
            building.ProductionTypes = new List<string>(stats.productionTypes);
            building.WaterCost = stats.waterCost;
            building.Width = stats.width;
            return building;
        }
    }
}
