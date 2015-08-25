using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using ZRTSModel.Entities;

namespace ZRTSModel.Factories
{
    /// <summary>
    /// This class will be used to give the correct stats to a unit based on the units type. It will read these stats 
    /// from an appropriate XML file located in "Content/units/file.xml" where "file" is the name of the xml file for the stats
    /// of an individual unit. Each unit will have its own file (i.e. soldier.xml, zombieBasic.xml, ect.)
    /// </summary>
    public class UnitFactory
    {

        private static UnitFactory instance;
        string BASE_DIR = "Content/units/";
        string unitList = "unitList.xml";
        List<string> unitPrefixes;
        Dictionary<string, UnitStats> stats;

        private UnitFactory()
        {
            unitPrefixes = new List<string>();
            stats = new Dictionary<string, UnitStats>();
            parseUnitList();
            loadAllUnitsStats();
        }

        public static UnitFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnitFactory();
                }
                return instance;
            }
        }

        /// <summary>
        /// Given a Unit an a string type, this function will give the unit the UnitStats of that type of Unit.
        /// </summary>
        /// <param name="unit">The Unit being given the UnitStats</param>
        /// <param name="type">denotes what type of UnitStats to use.</param>
        /// <returns></returns>
        public Unit createUnit(Unit unit, string type)
        {
            unit.stats = stats[type];
            unit.health = unit.stats.maxHealth;
            return unit;
        }

        /// <summary>
        /// This function will create a new Unit that belongs to 'owner' and has the UnitStats belonging to 'type'
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        /// <returns>A new Unit.</returns>
        public Unit createUnit(Player.Player owner, string type)
        {
            Unit u = new Unit(owner, stats[type]);
            u.health = u.stats.maxHealth;
            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns>The UnitStats corresponding to 'prefix'</returns>
        public UnitStats getStats(string prefix)
        {
            return stats[prefix];
        }

        private void loadAllUnitsStats()
        {
            foreach (string s in unitPrefixes)
            {
                loadUnitStats(stats[s], s + ".xml");
            }
        }

        private void loadUnitStats(UnitStats stats, string fileName)
        {
            string xml = readFile(BASE_DIR + fileName);
            parseXML(stats, xml);
        }

        private void parseUnitList()
        {
            string xml = readFile(BASE_DIR + unitList);
            bool endOfList = false;
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            while (!endOfList)
            {
                try
                {
                    reader.ReadToFollowing("Unit");
                    string s = reader.ReadElementContentAsString();
                    unitPrefixes.Add(s);
                    UnitStats uStat = new UnitStats();
                    uStat.type = s;
                    stats.Add(s, uStat);
                }
                catch
                {
                    endOfList = true;
                }
            }

            reader.Close();
        }


        private void parseXML(UnitStats stats, string xml)
        {
            XmlReader reader = XmlReader.Create(new StringReader(xml));

            // maxHealth
            reader.ReadToFollowing("maxHealth");
            stats.maxHealth = (short)reader.ReadElementContentAsInt();

            // speed
            reader.ReadToFollowing("speed");
            stats.speed = reader.ReadElementContentAsFloat();

            // attackRange
            reader.ReadToFollowing("attackRange");
            stats.attackRange = reader.ReadElementContentAsFloat();

            // attack
            reader.ReadToFollowing("attack");
            stats.attack = (short)reader.ReadElementContentAsInt();

            // attackTicks
            reader.ReadToFollowing("attackTicks");
            stats.attackTicks = (byte)reader.ReadElementContentAsInt();

            //visibilityRange
            reader.ReadToFollowing("visibilityRange");
            stats.visibilityRange = reader.ReadElementContentAsFloat();

            //buildSpeed
            reader.ReadToFollowing("buildSpeed");
            stats.buildSpeed = (byte)reader.ReadElementContentAsInt();

            //waterCost
            reader.ReadToFollowing("waterCost");
            stats.waterCost = (byte)reader.ReadElementContentAsInt();

            //foodCost
            reader.ReadToFollowing("foodCost");
            stats.foodCost = (byte)reader.ReadElementContentAsInt();

            //lumberCost
            reader.ReadToFollowing("lumberCost");
            stats.lumberCost = (byte)reader.ReadElementContentAsInt();

            //metalCost
            reader.ReadToFollowing("metalCost");
            stats.metalCost = (byte)reader.ReadElementContentAsInt();

            //canAttack
            reader.ReadToFollowing("canAttack");
            stats.canAttack = reader.ReadElementContentAsBoolean();

            //canHarvest
            reader.ReadToFollowing("canHarvest");
            stats.canHarvest = reader.ReadElementContentAsBoolean();

            //canBuild
            reader.ReadToFollowing("canBuild");
            stats.canBuild = reader.ReadElementContentAsBoolean();

            //isZombie
            reader.ReadToFollowing("isZombie");
            stats.isZombie = reader.ReadElementContentAsBoolean();

        }


        private string readFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string input = "";
            try
            {
                do
                {
                    input+= (reader.ReadLine());
                }
                while (reader.Peek() != -1);
            }

            catch
            {
            }

            finally
            {
                reader.Close();
            }

            return input;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns a list of strings where each string corresponds to a type of UnitStats.</returns>
        public List<string> getPrefixes()
        {
            return this.unitPrefixes;
        }

        /// <summary>
        /// Given a string, this function will create a new UnitStats corresponding to that string in the UnitStats dictionary.
        /// </summary>
        /// <param name="unitName"></param>
        public void addUnit(string unitName)
        {
            UnitStats s = new UnitStats();
            unitPrefixes.Add(unitName);
            stats.Add(unitName, s);
        }

        public void outputUnits()
        {
            this.outputUnitList();
        }

        private void outputUnitList()
        {
            using (XmlWriter listWriter = XmlWriter.Create("unitList.xml"))
            {

                listWriter.WriteStartElement("UnitList");

                foreach (string s in unitPrefixes)
                {
                    listWriter.WriteElementString("Unit", s);
                    this.outputUnit(s);
                }

                listWriter.WriteEndElement();
            }

        }

        private void outputUnit(string u)
        {
            UnitStats s = stats[u];

            using (XmlWriter listWriter = XmlWriter.Create(u + ".xml"))
            {
                listWriter.WriteStartElement("Unit");

                listWriter.WriteElementString("maxHealth", s.maxHealth.ToString());
                listWriter.WriteElementString("speed", s.speed.ToString());
                listWriter.WriteElementString("attackRange", s.attackRange.ToString());
                listWriter.WriteElementString("attack", s.attack.ToString());
				listWriter.WriteElementString("attackTicks", s.attackTicks.ToString());
				listWriter.WriteElementString("visibilityRange", s.visibilityRange.ToString());
				listWriter.WriteElementString("buildSpeed", s.buildSpeed.ToString());
				listWriter.WriteElementString("waterCost", s.waterCost.ToString());
				listWriter.WriteElementString("foodCost", s.foodCost.ToString());
				listWriter.WriteElementString("lumberCost", s.lumberCost.ToString());
				listWriter.WriteElementString("metalCost", s.metalCost.ToString());
                listWriter.WriteElementString("canAttack", s.canAttack.ToString());
                listWriter.WriteElementString("canHarvest", s.canHarvest.ToString());
                listWriter.WriteElementString("canBuild", s.canBuild.ToString());
				listWriter.WriteElementString("isZombie", s.isZombie.ToString());

                listWriter.WriteEndElement();
            }

        }
    }
}
