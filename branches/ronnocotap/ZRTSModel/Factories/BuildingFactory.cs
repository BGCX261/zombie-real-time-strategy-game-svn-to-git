using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ZRTSModel;
using ZRTSModel.Entities;
using ZRTSModel.Exception;

namespace ZRTSModel.Factories
{
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
                throw new FactoryException(e.Message);
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new FactoryException(e.Message);
            }


            return input;
        }

        public List<string> getBuildingTypes()
        {
            return this.buildingTypes;
        }

        public BuildingStats getStats(string type)
        {
            return statsDict[type];
        }
    }
}
