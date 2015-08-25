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
        List<string> buildingTypes;
        Dictionary<string, BuildingStats> statsDict;
		static BuildingFactory instance;

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

                Console.WriteLine(stats.ToString());
            }

            
        }

        private void readBuildingXML(string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            reader.ReadToFollowing("type");
            string type = reader.ReadElementContentAsString();

			BuildingStats stats = statsDict[type];

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

			// Read in what Units this Building can produce.
			bool endOfList = false;
			while (!endOfList)
			{
				try
				{
					reader.ReadToFollowing("Unit");
					string unitType = reader.ReadElementContentAsString();
					stats.productionTypes.Add(unitType);
				}
				catch
				{
					endOfList = true;
				}
			}

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

		/// <summary>
		/// 
		/// </summary>
		/// <returns>a List of strings where each string defines the "type" of a BuildingStats object.</returns>
        public List<string> getBuildingTypes()
        {
            return this.buildingTypes;
        }

		/// <summary>
		/// </summary>
		/// <param name="type">a string defining the "type" of a BuildingStats object.</param>
		/// <returns>The BuildingStats object defined by type.</returns>
        public BuildingStats getStats(string type)
        {
            return statsDict[type];
        }
    }
}
